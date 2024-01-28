using UnityEngine;

namespace GuitarHero
{
    public class GuitarHeroNoteObject : MonoBehaviour
    {
        public GuitarHeroNote.NoteType noteType = GuitarHeroNote.NoteType.Strum;
        public float time;
        public bool active = true;

        public void Hit()
        {
            active = false;
            gameObject.SetActive(false);
        }

        public void Missed()
        {
            active = false;
            gameObject.SetActive(false);
        }
    }
}