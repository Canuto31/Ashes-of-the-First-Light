using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions _playerInputActions;

    public Vector2 MoveInput { get; private set; }
    public bool DashPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }

    private void Awake() {
        _playerInputActions = new InputSystem_Actions();
    }

    private void OnEnable() {
        _playerInputActions.Player.Enable();
        
        _playerInputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _playerInputActions.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        _playerInputActions.Player.Jump.performed += ctx => {
            JumpPressed = true;
            JumpHeld = true;
        };
        _playerInputActions.Player.Jump.canceled += ctx => JumpHeld = false;

        _playerInputActions.Player.Dash.performed += ctx => DashPressed = true;
        _playerInputActions.Player.Attack.performed += ctx => AttackPressed = true;
    }
    
    private void OnDisable() {
        _playerInputActions.Player.Disable();
    }

    private void LateUpdate() {
        // Reset the input booleans
        JumpPressed = false;
        DashPressed = false;
        AttackPressed = false;
    }
}
