using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Provides the runtime behavior and data owned by the player input handler component.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions _playerInputActions;

    public Vector2 MoveInput { get; private set; }
    public bool DashPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool ToggleLanternPressed { get; private set; }

    //UI
    public bool ToggleMenuPressed { get; private set; }
    public bool NextPagePressed { get; private set; }
    public bool PreviousPagePressed { get; private set; }
    public bool NextBookPagePressed { get; private set; }
    public bool PreviousBookPagePressed { get; private set; }
    public bool NavigateUpPressed { get; private set; }
    public bool NavigateDownPressed { get; private set; }
    public bool ConfirmPressed { get; private set; }

    public bool OpenItemPressed { get; private set; }

    public bool IncreaseHealthPressed { get; private set; }
    public bool DecreaseHealthPressed { get; private set; }
    public bool RestoreSolarEnergyPressed { get; private set; }
    public bool ConsumeSolarEnergyPressed { get; private set; }
    public bool LockSlotPressed { get; private set; }
    public bool UnlockSlotPressed { get; private set; }

    private void Awake()
    {
        _playerInputActions = new InputSystem_Actions();
        RegisterInputCallbacks();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        _playerInputActions?.Dispose();
    }

    private void RegisterInputCallbacks()
    {
        var playerActions = _playerInputActions.Player;

        playerActions.Move.performed += context => MoveInput = context.ReadValue<Vector2>();
        playerActions.Move.canceled += _ => MoveInput = Vector2.zero;

        playerActions.Jump.performed += _ =>
        {
            JumpPressed = true;
            JumpHeld = true;
        };
        playerActions.Jump.canceled += _ => JumpHeld = false;

        playerActions.Dash.performed += _ => DashPressed = true;
        playerActions.Sprint.performed += _ => SprintHeld = true;
        playerActions.Sprint.canceled += _ => SprintHeld = false;
        playerActions.Attack.performed += _ => AttackPressed = true;
        playerActions.Interact.performed += _ => InteractPressed = true;
        playerActions.ToggleLantern.performed += _ => ToggleLanternPressed = true;
        playerActions.ToggleMenu.performed += _ => ToggleMenuPressed = true;
        playerActions.NextPage.performed += _ => NextPagePressed = true;
        playerActions.PreviousPage.performed += _ => PreviousPagePressed = true;
        playerActions.NextBookPage.performed += _ => NextBookPagePressed = true;
        playerActions.PreviousBookPage.performed += _ => PreviousBookPagePressed = true;
        playerActions.NavigateUp.performed += _ => NavigateUpPressed = true;
        playerActions.NavigateDown.performed += _ => NavigateDownPressed = true;
        playerActions.Confirm.performed += _ => ConfirmPressed = true;
        playerActions.OpenItem.performed += _ => OpenItemPressed = true;
        playerActions.IncreaseHealth.performed += _ => IncreaseHealthPressed = true;
        playerActions.DecreaseHealth.performed += _ => DecreaseHealthPressed = true;
        playerActions.RestoreSolarEnergy.performed += _ => RestoreSolarEnergyPressed = true;
        playerActions.ConsumeSolarEnergy.performed += _ => ConsumeSolarEnergyPressed = true;
        playerActions.LockSlot.performed += _ => LockSlotPressed = true;
        playerActions.UnlockSlot.performed += _ => UnlockSlotPressed = true;
    }

    private void LateUpdate()
    {
        ResetFrameInput();
    }

    private void ResetFrameInput()
    {
        // Reset commands after every Update consumer has had a chance to read them.
        JumpPressed = false;
        DashPressed = false;
        AttackPressed = false;
        InteractPressed = false;
        ToggleLanternPressed = false;
        ToggleMenuPressed = false;
        NextPagePressed = false;
        PreviousPagePressed = false;

        NextBookPagePressed = false;
        PreviousBookPagePressed = false;
        NavigateUpPressed = false;
        NavigateDownPressed = false;
        ConfirmPressed = false;

        OpenItemPressed = false;
        
        IncreaseHealthPressed = false;
        DecreaseHealthPressed = false;
        
        RestoreSolarEnergyPressed = false;
        ConsumeSolarEnergyPressed = false;
        
        LockSlotPressed = false;
        UnlockSlotPressed = false;
    }

    public bool ConsumeToggleMenu()
    {
        if (!ToggleMenuPressed)
            return false;

        ToggleMenuPressed = false;
        return true;
    }
}
