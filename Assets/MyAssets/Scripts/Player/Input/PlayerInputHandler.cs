using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        _playerInputActions.Player.Jump.performed += ctx =>
        {
            JumpPressed = true;
            JumpHeld = true;
        };
        _playerInputActions.Player.Jump.canceled += ctx => JumpHeld = false;

        _playerInputActions.Player.Dash.performed += ctx => DashPressed = true;
        
        _playerInputActions.Player.Sprint.performed += ctx =>
        {
            SprintHeld = true;
        };

        _playerInputActions.Player.Sprint.canceled += ctx =>
        {
            SprintHeld = false;
        };
        _playerInputActions.Player.Attack.performed += ctx => AttackPressed = true;

        _playerInputActions.Player.Interact.performed += ctx => InteractPressed = true;

        _playerInputActions.Player.ToggleLantern.performed += ctx => ToggleLanternPressed = true;

        _playerInputActions.Player.ToggleMenu.performed += ctx => ToggleMenuPressed = true;

        _playerInputActions.Player.NextPage.performed += ctx => NextPagePressed = true;
        _playerInputActions.Player.PreviousPage.performed += ctx => PreviousPagePressed = true;

        _playerInputActions.Player.NextBookPage.performed += ctx => NextBookPagePressed = true;
        _playerInputActions.Player.PreviousBookPage.performed += ctx => PreviousBookPagePressed = true;

        _playerInputActions.Player.NavigateUp.performed += ctx => NavigateUpPressed = true;
        _playerInputActions.Player.NavigateDown.performed += ctx => NavigateDownPressed = true;

        _playerInputActions.Player.Confirm.performed += ctx => ConfirmPressed = true;

        _playerInputActions.Player.OpenItem.performed += ctx => OpenItemPressed = true;

        _playerInputActions.Player.IncreaseHealth.performed += ctx => IncreaseHealthPressed = true;
        _playerInputActions.Player.DecreaseHealth.performed += ctx => DecreaseHealthPressed = true;
        
        _playerInputActions.Player.RestoreSolarEnergy.performed += ctx => RestoreSolarEnergyPressed = true;
        _playerInputActions.Player.ConsumeSolarEnergy.performed += ctx => ConsumeSolarEnergyPressed = true;
        
        _playerInputActions.Player.LockSlot.performed += ctx => LockSlotPressed = true;
        _playerInputActions.Player.UnlockSlot.performed += ctx => UnlockSlotPressed = true;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    private void LateUpdate()
    {
        // Reset the input booleans
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
        if (!ToggleMenuPressed) return false;

        ToggleMenuPressed = false;
        return true;
    }
}