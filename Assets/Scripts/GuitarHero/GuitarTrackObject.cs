using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuitarHero
{
    public class GuitarTrackObject : MonoBehaviour
    {
        public GuitarHeroNoteObject notePrefab, tapNotePrefab, slideNotePrefab;
        public List<GuitarHeroNoteObject> notes = new();
        public void AddNote(double time, GuitarHeroNote.NoteType noteType, float fretboardTime, float fretboardLength)
        {
            var note = Instantiate(noteType switch
            {
                GuitarHeroNote.NoteType.Strum => notePrefab,
                GuitarHeroNote.NoteType.Tap => tapNotePrefab,
                GuitarHeroNote.NoteType.Slide => slideNotePrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(noteType), noteType, null)
            }, transform);
            note.transform.localPosition =
                new Vector3(0.0F, 0.0F, (((float)time / fretboardTime) * fretboardLength));
            note.noteType = noteType;
            note.time = (float)time;
            notes.Add(note);
        }
    }
}