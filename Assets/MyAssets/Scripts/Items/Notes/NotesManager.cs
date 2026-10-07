using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates notes state and operations for the game.
/// </summary>
public class NotesManager : MonoBehaviour
{
    public static NotesManager Instance { get; private set; }

    private readonly List<NoteData> _notes = new();
    
    private NoteData _lastCollectedNote;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddNote(NoteData note)
    {
        if (note == null || _notes.Contains(note))
            return;

        _notes.Add(note);
        _lastCollectedNote = note;

        Debug.Log("Note added: " + note.noteTitle);
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
