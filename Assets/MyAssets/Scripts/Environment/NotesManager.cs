using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance;
    
    private List<NoteData> _notes = new List<NoteData>();
    
    private NoteData _lastCollectedNote;

    private void Awake()
    {
        Instance = this;
    }

    public void AddNote(NoteData note)
    {
        if (!_notes.Contains(note))
        {
            _notes.Add(note);
            
            _lastCollectedNote = note;
            
            Debug.Log("Note added: " + note.noteTitle);
        }
    }

    public List<NoteData> GetNotes()
    {
        return _notes;
    }

    public NoteData GetLastCollectedNote()
    {
        return _lastCollectedNote;
    }
}
