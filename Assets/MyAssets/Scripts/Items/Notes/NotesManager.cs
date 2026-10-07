using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coordinates notes state and operations for the game.
/// </summary>
public class NotesManager : MonoBehaviour
{

    #region Fields and Configuration

    public static NotesManager Instance { get; private set; }

    private readonly List<NoteData> _notes = new();
    
    private NoteData _lastCollectedNote;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Adds note for this component.
    /// </summary>
    public void AddNote(NoteData note)
    {
        if (note == null || _notes.Contains(note))
            return;

        _notes.Add(note);
        _lastCollectedNote = note;

        Debug.Log("Note added: " + note.noteTitle);
    }

    /// <summary>
    /// Returns the current notes for this component.
    /// </summary>
    public List<NoteData> GetNotes()
    {
        return _notes;
    }

    /// <summary>
    /// Returns the current last collected note for this component.
    /// </summary>
    public NoteData GetLastCollectedNote()
    {
        return _lastCollectedNote;
    }

    #endregion
}
