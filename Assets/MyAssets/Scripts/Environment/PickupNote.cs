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
        
        UI_Interaction.Instance.ShowTextTimed(_note.noteTitle + " acquired", 1.5f);
        
        Destroy(gameObject);
        
        NotesUIManager.Instance.OpenNote(_note);
    }
}
