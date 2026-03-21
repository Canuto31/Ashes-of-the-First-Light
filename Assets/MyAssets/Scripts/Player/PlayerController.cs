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

    [Header("Wall Check")]
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckDistance = 0.5f;

    private bool _isTouchingWall;
    private bool _wasTouchingWall;
    private int _wallDir; // -1 izquierda, 1 derecha

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    private bool _isWallSliding;

    [Header("Wall Jump")]
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;
    public float wallJumpLockTime = 0.2f;

    private bool _isWallJumping;
    private float _wallJumpLockCounter;

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
        CheckWall();
        UpdateJumpBuffer();
        HandleWallJumpLock();
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyBetterGravity();
        HandleWallSlide();
    }

    // --------------------
    // MOVEMENT
    // --------------------
    private void HandleMovement()
    {
        if (_isWallJumping || _isWallSliding) return;

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
        // WALL JUMP (PRIORIDAD)
        if (_jumpBufferCounter > 0f && _isWallSliding)
        {
            _rb.linearVelocity = new Vector2(
                -_wallDir * wallJumpForceX,
                wallJumpForceY
            );

            _isWallJumping = true;
            _wallJumpLockCounter = wallJumpLockTime;

            _jumpBufferCounter = 0f;
            return;
        }

        // SALTO BASE (coyote)
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f && !_hasJumped)
        {
            Jump();
            _hasJumped = true;
            _coyoteTimeCounter = 0f;
        }
        // DOUBLE JUMP
        else if (_jumpBufferCounter > 0f && _extraJumpsRemaining > 0 && !_isGrounded)
        {
            Jump();
            _extraJumpsRemaining--;
        }

        // CORTE DE SALTO
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
    // WALL SLIDE
    // --------------------
    private void HandleWallSlide()
    {
        float inputDir = _input.MoveInput.x;

        bool pushingToWall = _isTouchingWall && inputDir == _wallDir;

        if (pushingToWall && !_isGrounded && _rb.linearVelocityY < 0)
        {
            _isWallSliding = true;

            _rb.linearVelocity = new Vector2(
                _rb.linearVelocityX,
                -wallSlideSpeed
            );
        }
        else
        {
            _isWallSliding = false;
        }
    }

    // --------------------
    // WALL JUMP LOCK
    // --------------------
    private void HandleWallJumpLock()
    {
        if (_isWallJumping)
        {
            _wallJumpLockCounter -= Time.deltaTime;

            if (_wallJumpLockCounter <= 0f)
            {
                _isWallJumping = false;
            }
        }
    }

    // --------------------
    // WALL CHECK
    // --------------------
    private void CheckWall()
    {
        bool hitRight = Physics2D.Raycast(
            wallCheckRight.position,
            Vector2.right,
            wallCheckDistance,
            groundLayer
        );

        bool hitLeft = Physics2D.Raycast(
            wallCheckLeft.position,
            Vector2.left,
            wallCheckDistance,
            groundLayer
        );

        bool touchingWallNow = hitRight || hitLeft;

        if (hitRight) _wallDir = 1;
        else if (hitLeft) _wallDir = -1;
        else _wallDir = 0;

        // Evento: recargar saltos al tocar pared
        if (touchingWallNow && !_wasTouchingWall && !_isGrounded)
        {
            _extraJumpsRemaining = maxExtraJumps;
        }

        _isTouchingWall = touchingWallNow;
        _wasTouchingWall = touchingWallNow;
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
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                  (gravityMultiplier - 1) * Time.fixedDeltaTime;
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
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                wallCheckLeft.position,
                wallCheckLeft.position + Vector3.left * wallCheckDistance
            );
        }

        if (wallCheckRight != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                wallCheckRight.position,
                wallCheckRight.position + Vector3.right * wallCheckDistance
            );
        }
    }
}