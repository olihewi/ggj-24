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

        private float time = 0.0F;

        private void Start()
        {
            InitializeNotes();
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
                    t.AddNote(marker.time, marker.noteType, fretboardLength, fretboardTime);
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
            tracksParent.localPosition = new Vector3(0.0F, 0.0F, -((time / fretboardTime) % 1.0F) * fretboardLength);
            fretboard.localPosition = new Vector3(0.0F, 0.0F, -((time / fretboardTime)%1.0F) * fretboardLength);
            
        }
    }
}