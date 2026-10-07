/// <summary>
/// Defines the available player state values used by the game.
/// </summary>
public enum PlayerState
{
    Grounded,
    Airborne,
    WallSliding,
    Dashing,
    Attacking
}

/// <summary>
/// Provides the runtime behavior and data owned by the player state machine component.
/// </summary>
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
    }
}
