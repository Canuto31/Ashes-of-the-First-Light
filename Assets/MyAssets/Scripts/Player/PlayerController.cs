using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputHandler _input;

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

    [Header("Jump Assist")]
    public float coyoteTime = 0.1f;
    private float _coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.1f;
    private float _jumpBufferCounter;

    [Header("Extra Jumps")]
    public int maxExtraJumps = 1;
    private int _extraJumpsRemaining;

    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _hasJumped;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        CheckGround();
        UpdateJumpBuffer();
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyBetterGravity();
    }

    // --------------------
    // MOVEMENT
    // --------------------
    private void HandleMovement()
    {
        float targetSpeed = _input.MoveInput.x * maxSpeed;
        float speedDiff = targetSpeed - _rb.linearVelocityX;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        float movement = speedDiff * accelRate;

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocityX + movement * Time.fixedDeltaTime,
            _rb.linearVelocityY
        );
    }

    // --------------------
    // JUMP
    // --------------------
    private void HandleJump()
    {
        // SALTO BASE (suelo + coyote)
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f && !_hasJumped)
        {
            Jump();

            _hasJumped = true;
            _coyoteTimeCounter = 0f;
        }
        // DOUBLE JUMP (aire)
        else if (_jumpBufferCounter > 0f && _extraJumpsRemaining > 0 && !_isGrounded)
        {
            Jump();

            _extraJumpsRemaining--;
        }

        // CORTE DE SALTO (variable jump)
        if (!_input.JumpHeld && _rb.linearVelocityY > 0)
        {
            _rb.linearVelocity = new Vector2(
                _rb.linearVelocityX,
                _rb.linearVelocityY * 0.5f
            );
        }
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
        _jumpBufferCounter = 0f;
    }

    // --------------------
    // JUMP BUFFER
    // --------------------
    private void UpdateJumpBuffer()
    {
        if (_input.JumpPressed)
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
    }

    // --------------------
    // GRAVITY
    // --------------------
    private void ApplyBetterGravity()
    {
        if (_rb.linearVelocityY < 0)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --------------------
    // GROUND CHECK
    // --------------------
    private void CheckGround()
    {
        bool groundedNow = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Detectar aterrizaje (evento)
        if (groundedNow && !_wasGrounded)
        {
            _extraJumpsRemaining = maxExtraJumps;
            _hasJumped = false;
        }

        _isGrounded = groundedNow;

        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        _wasGrounded = _isGrounded;
    }

    // --------------------
    // DEBUG
    // --------------------
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}