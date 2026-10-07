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

        NotesManager.Instance?.AddNote(_note);

        BookMenuManager.Instance?.QueueContextPage(
            BookMenuManager.BookPage.Notes
        );

        if (_readingTutorial != null && TutorialManager.Instance != null &&
            !TutorialManager.Instance.HasSeen(_readingTutorial.tutorialId))
        {
            StartCoroutine(
                FirstNoteSequence(_readingTutorial.tutorialId)
            );
        }
        else
        {
            UI_Interaction.Instance?.ShowTextTimed(
                _note.noteTitle + " acquired\nPress TAB to read",
                2f
            );

            Destroy(gameObject);
        }
    }
    
    private IEnumerator FirstNoteSequence(string tutorialId)
    {
        TutorialUIManager.Instance?.ShowTutorial(
            _readingTutorial
        );

        yield return new WaitUntil(() =>
            GameStateManager.Instance.IsPlaying()
        );

        UI_Interaction.Instance?.ShowTextTimed(
            _note.noteTitle + " acquired\nPress TAB to read",
            2f
        );

        Destroy(gameObject);
    }
}
