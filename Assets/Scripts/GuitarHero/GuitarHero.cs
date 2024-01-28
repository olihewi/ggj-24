using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

namespace GuitarHero
{
    public class GuitarHero : MonoBehaviour
    {
        public PlayableDirector playableDirector;
        
        public Transform fretboard;
        public float fretboardTime = 2.0F;
        public float fretboardLength = 4.5F;

        public Transform tracksParent;
        public GuitarTrackObject[] tracks;
        public GuitarFretNote[] fretNotes;

        public float noteThreshold = 0.25F;

        public AudioSource failSound;
        

        private float time = 0.0F;

        private void Start()
        {
            InitializeNotes();
            playableDirector.Play();
        }
        private void InitializeNotes()
        {
            if (playableDirector == null || playableDirector.playableAsset is not TimelineAsset timeline) return;
            foreach (var track in timeline.GetRootTracks().OfType<GuitarTrack>())
            {
                var t = tracks[track.index];
                if (t == null) continue;
                foreach (var marker in track.GetMarkers().OfType<GuitarHeroNote>())
                {
                    t.AddNote(marker.time, marker.noteType, fretboardTime, fretboardLength);
                }
            }
        }

        private void Update()
        {
            UpdateTime();
        }

        public bool HasStrummed => Mathf.Abs(Input.GetAxisRaw("Strum")) > 0.01F || Input.GetKeyDown(KeyCode.Alpha1) ||
                                   Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) ||
                                   Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5);

        public GuitarHeroNoteObject[] shouldHit = new GuitarHeroNoteObject[5];
        private void UpdateTime()
        {
            time = (float)playableDirector.time;
            tracksParent.localPosition = new Vector3(tracksParent.localPosition.x, tracksParent.localPosition.y, -(time / fretboardTime * fretboardLength));
            fretboard.localPosition = new Vector3(fretboard.localPosition.x, fretboard.localPosition.y, -((time / fretboardTime)%1.0F) * fretboardLength);

            for (int i = 0; i < shouldHit.Length; i++)
            {
                shouldHit[i] = null;
            }
            
            for (int i = 0; i < tracks.Length; i++)
            {
                var track = tracks[i];
                for (int j = track.notes.Count - 1; j >= 0; j--)
                {
                    var note = track.notes[j];
                    if (!note.active) continue;
                    var diff = note.time - time;
                    if (Mathf.Abs(diff) <= noteThreshold)
                    {
                        shouldHit[i] = note;
                    }
                    else if (diff < -noteThreshold)
                    {
                        note.Missed();
                        failSound.pitch = Random.Range(0.9F, 1.1F);
                        failSound.PlayOneShot(failSound.clip);
                    }
                }
            }

            bool hitNotes = HasHitNotes;
            if (hitNotes)
            {
                for (int i = 0; i < shouldHit.Length; i++)
                {
                    if (shouldHit[i] != null)
                    {
                        shouldHit[i].Hit();
                        fretNotes[i].Strum();
                    }
                }
            }
            else if (HasStrummed)
            {
                failSound.pitch = Random.Range(0.9F, 1.1F);
                failSound.PlayOneShot(failSound.clip);
            }

        }

        public bool HasHitNotes
        {
            get
            {
                bool hasStrummed = HasStrummed;
                int nullCount = 0;
                for (int i = 0; i < shouldHit.Length; i++)
                {
                    if (shouldHit[i] == null)
                    {
                        nullCount++;
                        continue;
                    }

                    if (shouldHit[i].noteType == GuitarHeroNote.NoteType.Strum && !hasStrummed)
                    {
                        return false;
                    }

                    for (int j = i + 1; j < shouldHit.Length; j++)
                    {
                        if (shouldHit[j] == null && fretNotes[j].IsHeld) return false;
                    }
                }

                if (nullCount == shouldHit.Length) return false;
                return true;
            }
        }
    }
}