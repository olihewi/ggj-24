using System.Collections.Generic;
using UnityEngine;

namespace GuitarHero
{
    public class GuitarTrackObject : MonoBehaviour
    {
        public GuitarHeroNoteObject notePrefab;
        public List<GuitarHeroNoteObject> notes = new();
        public void AddNote(double time, GuitarHeroNote.NoteType noteType, float fretboardTime, float fretboardLength)
        {
            var note = Instantiate(notePrefab, transform);
            note.transform.localPosition =
                new Vector3(0.0F, 0.0F, (((float)time / fretboardTime) * fretboardLength));
            note.noteType = noteType;
            note.time = (float)time;
            notes.Add(note);
        }
    }
}