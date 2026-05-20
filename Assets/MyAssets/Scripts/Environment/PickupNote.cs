using UnityEngine;

public class PickupNote : MonoBehaviour, IInteractable
{
    [SerializeField] private NoteData _note;

    public string GetInteractionText()
    {
        return "Read " + _note.noteTitle;
    }

    public void Interact()
    {
        if (!GameStateManager.Instance.IsPlaying())
            return;

        NotesManager.Instance.AddNote(_note);

        string tutorialId = "NOTE_READING";

        if (!TutorialManager.Instance.HasSeen(tutorialId))
        {
            TutorialUIManager.Instance.ShowTutorial(
                tutorialId,
                "Use A / D to change pages"
            );
        }

        UI_Interaction.Instance.ShowTextTimed(
            _note.noteTitle + " acquired\nPress TAB to read",
            2f
        );

        BookMenuManager.Instance.QueueContextPage(
            BookMenuManager.BookPage.Notes
        );

        Destroy(gameObject);
    }
}