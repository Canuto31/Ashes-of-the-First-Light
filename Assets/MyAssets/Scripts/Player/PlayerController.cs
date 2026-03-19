using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputHandler _playerInputHandler;

    [Header("Movement")]
    public float maxSpeed = 6f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float gravityMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool _isGrounded;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update() {
        CheckGround();
        HandleJump();
    }

    private void FixedUpdate() {
        HandleMovement();
        ApplyBetterGravity();
    }

    // --------------------
    // MOVEMENT
    // --------------------
    private void HandleMovement() {
        float targetSpeed = _playerInputHandler.MoveInput.x * maxSpeed;
        float speedDiff = targetSpeed - _rb.linearVelocityX;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        float movement = speedDiff * accelRate;

        _rb.linearVelocity = new Vector2(_rb.linearVelocityX + movement * Time.fixedDeltaTime, _rb.linearVelocityY);
    }

    // --------------------
    // JUMP
    // --------------------
    private void HandleJump() {
        if (_playerInputHandler.JumpPressed && _isGrounded) {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
        }

        // Corte de salto (Salto variable)
        if (!_playerInputHandler.JumpHeld && _rb.linearVelocityY > 0) {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _rb.linearVelocityY * 0.5f);
        }
    }

    // --------------------
    // GRAVITY
    // --------------------
    private void ApplyBetterGravity() {
        if (_rb.linearVelocityY < 0) {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --------------------
    // GROUND CHECK
    // --------------------
    private void CheckGround() {
        _isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void OnDrawGizmosSelected() {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
