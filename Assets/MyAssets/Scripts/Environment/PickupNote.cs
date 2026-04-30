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
        if (!GameStateManager.Instance.IsPlaying()) return;

        NotesManager.Instance.AddNote(_note);
        
        UI_Interaction.Instance.ShowTextTimed(_note.noteTitle + " acquired\nPress E to read", 2f);
        
        string tutorialId = "NOTE_READING";

        if (!TutorialManager.Instance.HasSeen(tutorialId))
        {
            TutorialUIManager.Instance.ShowTutorial(tutorialId, "Use A / D to change pages\nPress E to exit", () =>
            {
                NotesUIManager.Instance.OpenNote(_note);
            });
        }
        else
        {
            NotesUIManager.Instance.OpenNote(_note);
        }
        
        Destroy(gameObject);
    }
}
