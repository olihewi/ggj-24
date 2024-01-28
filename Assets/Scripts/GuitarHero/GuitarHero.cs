using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

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

        public float noteThreshold = 0.25F;
        

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

        private void UpdateTime()
        {
            time = (float)playableDirector.time;
            tracksParent.localPosition = new Vector3(tracksParent.localPosition.x, tracksParent.localPosition.y, -(time / fretboardTime * fretboardLength));
            fretboard.localPosition = new Vector3(fretboard.localPosition.x, fretboard.localPosition.y, -((time / fretboardTime)%1.0F) * fretboardLength);

            for (int i = 0; i < tracks.Length; i++)
            {
                var track = tracks[i];
                foreach (var note in track.notes)
                {
                    if (!note.active) continue;
                    var diff = note.time - time;
                    if (Mathf.Abs(diff) <= noteThreshold)
                    {
                        note.Hit();
                    }
                    else if (diff < -noteThreshold)
                    {
                        note.Missed();
                    }
                }
            }
            
        }
    }
}