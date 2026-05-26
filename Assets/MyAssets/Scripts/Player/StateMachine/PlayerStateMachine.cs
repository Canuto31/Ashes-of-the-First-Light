using UnityEngine;

public enum PlayerState
{
    Grounded,
    Airborne,
    WallSliding,
    Dashing
}

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void ChangeState(PlayerState newState)
    {
        CurrentState = newState;
    }
}
