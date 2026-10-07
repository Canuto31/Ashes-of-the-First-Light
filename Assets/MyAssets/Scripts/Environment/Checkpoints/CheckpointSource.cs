using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the checkpoint source component.
/// </summary>
public class CheckpointSource : MonoBehaviour, IInteractable
{

    #region Fields and Configuration

    [SerializeField] private Transform _respawnPoint;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Returns the prompt text presented for this interaction.
    /// </summary>
    public string GetInteractionText()
    {
        return "Rest at the fountain";
    }

    /// <summary>
    /// Executes this object's response to a valid player interaction.
    /// </summary>
    public void Interact()
    {
        if (GameStateManager.Instance == null || !GameStateManager.Instance.IsPlaying())
            return;

        Vector3 point = _respawnPoint != null ? _respawnPoint.position : transform.position;

        CheckpointManager.Instance?.SetCheckpoint(point);
        UI_Interaction.Instance?.ShowTextTimed("Checkpoint saved", 1.5f);
    }

    #endregion
}
