using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player animation events component.
/// </summary>
public class PlayerAnimationEvents : MonoBehaviour
{

    #region Fields and Configuration

    private PlayerController _playerController;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Releases the active attack lock when the animation event completes.
    /// </summary>
    public void EndAttack()
    {
        if (_playerController != null)
            _playerController.EndAttack();
    }

    #endregion
}
