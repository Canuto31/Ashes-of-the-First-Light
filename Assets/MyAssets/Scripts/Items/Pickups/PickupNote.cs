using System.Collections;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the pickup note component.
/// </summary>
public class PickupNote : MonoBehaviour, IInteractable
{

    #region Fields and Configuration

    [SerializeField] 
    private NoteData _note;
    
    [SerializeField]
    private TutorialData _readingTutorial;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Returns the prompt text presented for this interaction.
    /// </summary>
    public string GetInteractionText()
    {
        return _note != null ? _note.noteTitle : "Read note";
    }

    /// <summary>
    /// Executes this object's response to a valid player interaction.
    /// </summary>
    public void Interact()
    {
        if (_note == null || GameStateManager.Instance == null || !GameStateManager.Instance.IsPlaying())
            return;

        CollectNote();

        if (ShouldShowReadingTutorial())
        {
            StartCoroutine(FirstNoteSequence());
            return;
        }

        CompletePickup();
    }

    /// <summary>
    /// Executes the collect note operation for this component.
    /// </summary>
    private void CollectNote()
    {
        NotesManager.Instance?.AddNote(_note);
        BookMenuManager.Instance?.QueueContextPage(BookMenuManager.BookPage.Notes);
    }

    /// <summary>
    /// Executes the should show reading tutorial operation for this component.
    /// </summary>
    private bool ShouldShowReadingTutorial()
    {
        return _readingTutorial != null &&
               TutorialManager.Instance != null &&
               !TutorialManager.Instance.HasSeen(_readingTutorial.tutorialId);
    }

    /// <summary>
    /// Executes the complete pickup operation for this component.
    /// </summary>
    private void CompletePickup()
    {
        UI_Interaction.Instance?.ShowTextTimed(
            _note.noteTitle + " acquired\nPress TAB to read",
            2f
        );

        Destroy(gameObject);
    }
    
    /// <summary>
    /// Executes the first note sequence operation for this component.
    /// </summary>
    private IEnumerator FirstNoteSequence()
    {
        TutorialUIManager.Instance?.ShowTutorial(
            _readingTutorial
        );

        yield return new WaitUntil(() =>
            GameStateManager.Instance.IsPlaying()
        );

        CompletePickup();
    }

    #endregion
}
