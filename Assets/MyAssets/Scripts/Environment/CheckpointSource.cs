using UnityEngine;

public class CheckpointSource : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _respawnPoint;

    public string GetInteractionText()
    {
        return "Rest at the fountain";
    }

    public void Interact()
    {
        if (!GameStateManager.Instance.IsPlaying()) return;
        
        Vector3 point = _respawnPoint != null ? _respawnPoint.position : transform.position;

        CheckpointManager.Instance.SetCheckpoint(point);
        
        UI_Interaction.Instance.ShowTextTimed("Checkpoint saved", 1.5f);
    }
}
