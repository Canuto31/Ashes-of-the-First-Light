using UnityEngine;

/// <summary>
/// Coordinates checkpoint state and operations for the game.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 _lastCheckpointPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpointPosition = position;
        
        Debug.Log("Checkpoint saved at: " + position);
    }

    public void ReturnToCheckpoint(GameObject player)
    {
        if (player == null)
            return;

        player.transform.position = _lastCheckpointPosition;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        Debug.Log("Returned to checkpoint");
    }
}
