using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 _lastCheckpointPosition;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpointPosition = position;
        
        Debug.Log("Checkpoint saved ad: " + position);
    }

    public void ReturnToCheckpoint(GameObject player)
    {
        player.transform.position = _lastCheckpointPosition;
        
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
        
        Debug.Log("Returned to checkpoint");
    }
}
