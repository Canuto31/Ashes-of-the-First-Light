using System.Collections;
using UnityEngine;

public class PickupNote : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private NoteData _note;
    
    [SerializeField]
    private TutorialData _readingTutorial;

    public string GetInteractionText()
    {
        return _note.noteTitle;
    }

    public void Interact()
    {
        if (!GameStateManager.Instance.IsPlaying())
            return;

        NotesManager.Instance.AddNote(_note);

        BookMenuManager.Instance.QueueContextPage(
            BookMenuManager.BookPage.Notes
        );

        if (!TutorialManager.Instance.HasSeen(
                _readingTutorial.tutorialId))
        {
            StartCoroutine(
                FirstNoteSequence(_readingTutorial.tutorialId)
            );
        }
        else
        {
            UI_Interaction.Instance.ShowTextTimed(
                _note.noteTitle + " acquired\nPress TAB to read",
                2f
            );

            Destroy(gameObject);
        }
    }
    
    private IEnumerator FirstNoteSequence(string tutorialId)
    {
        TutorialUIManager.Instance.ShowTutorial(
            _readingTutorial
        );

        yield return new WaitUntil(() =>
            GameStateManager.Instance.IsPlaying()
        );

        UI_Interaction.Instance.ShowTextTimed(
            _note.noteTitle + " acquired\nPress TAB to read",
            2f
        );

        Destroy(gameObject);
    }
}