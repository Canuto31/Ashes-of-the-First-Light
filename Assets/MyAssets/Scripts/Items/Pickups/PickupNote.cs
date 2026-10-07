using System.Collections;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the pickup note component.
/// </summary>
public class PickupNote : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private NoteData _note;
    
    [SerializeField]
    private TutorialData _readingTutorial;

    public string GetInteractionText()
    {
        return _note != null ? _note.noteTitle : "Read note";
    }

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

    private void CollectNote()
    {
        NotesManager.Instance?.AddNote(_note);
        BookMenuManager.Instance?.QueueContextPage(BookMenuManager.BookPage.Notes);
    }

    private bool ShouldShowReadingTutorial()
    {
        return _readingTutorial != null &&
               TutorialManager.Instance != null &&
               !TutorialManager.Instance.HasSeen(_readingTutorial.tutorialId);
    }

    private void CompletePickup()
    {
        UI_Interaction.Instance?.ShowTextTimed(
            _note.noteTitle + " acquired\nPress TAB to read",
            2f
        );

        Destroy(gameObject);
    }
    
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
}
