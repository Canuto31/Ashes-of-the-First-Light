using UnityEngine;

public class PauseMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    public void ReturnToCheckpoint()
    {
        CheckpointManager.Instance.ReturnToCheckpoint(_player);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }
}
