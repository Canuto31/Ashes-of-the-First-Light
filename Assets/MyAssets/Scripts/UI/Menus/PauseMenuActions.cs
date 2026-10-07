using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the pause menu actions component.
/// </summary>
public class PauseMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    public void ReturnToCheckpoint()
    {
        CheckpointManager.Instance?.ReturnToCheckpoint(_player);
        GameStateManager.Instance?.SetState(GameStateManager.GameState.Playing);
    }
}
