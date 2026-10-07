/// <summary>
/// Defines the available player state values used by the game.
/// </summary>
public enum PlayerState
{

    #region Values

    Grounded,
    Airborne,
    WallSliding,
    Dashing,
    Attacking

    #endregion
}

/// <summary>
/// Provides the runtime behavior and data owned by the player state machine component.
/// </summary>
public class PlayerStateMachine
{
    #region State

    public PlayerState CurrentState { get; private set; }

    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Executes the change state operation for this component.
    /// </summary>
    public void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
    }

    #endregion
}
