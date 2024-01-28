using UnityEngine;

namespace GuitarHero
{
    public class GuitarTrackObject : MonoBehaviour
    {
        public GuitarHeroNoteObject notePrefab;
        public void AddNote(double time, GuitarHeroNote.NoteType noteType, float fretboardTime, float fretboardLength)
        {
            Instantiate(notePrefab, transform);
            notePrefab.transform.localPosition =
                new Vector3(0.0F, 0.0F, (((float)time / fretboardTime) * fretboardLength));
        }
    }
}