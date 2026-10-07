using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player animation events component.
/// </summary>
public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    public void EndAttack()
    {
        if (_playerController != null)
            _playerController.EndAttack();
    }
}
