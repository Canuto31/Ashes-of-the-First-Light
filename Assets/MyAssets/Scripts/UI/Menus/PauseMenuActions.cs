using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the pause menu actions component.
/// </summary>
public class PauseMenuActions : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private GameObject _player;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Returns to checkpoint for this component.
    /// </summary>
    public void ReturnToCheckpoint()
    {
        CheckpointManager.Instance?.ReturnToCheckpoint(_player);
        GameStateManager.Instance?.SetState(GameStateManager.GameState.Playing);
    }

    #endregion
}
