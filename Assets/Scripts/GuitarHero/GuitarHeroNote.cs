using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace GuitarHero
{
    public class GuitarHeroNote : Marker
    {
        public enum NoteType
        {
            Strum,
            Tap,
            Slide
        }
        public NoteType noteType = NoteType.Strum;
    }
}