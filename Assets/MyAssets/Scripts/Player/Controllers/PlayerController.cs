using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInputHandler _input;
    private PlayerStateMachine _stateMachine;

    [Header("Movement")]
    public float maxSpeed = 6f;
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float sprintMultiplier = 1.5f;

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
    private int _wallDir;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    private bool _isWallSliding;

    [Header("Wall Jump")]
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;
    public float wallJumpLockTime = 0.2f;

    private bool _isWallJumping;
    private float _wallJumpLockCounter;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashStaminaCost = 25f;

    private bool _isDashing;
    private float _dashTimeCounter;

    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _hasJumped;
    
    private PlayerStamina _playerStamina;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputHandler>();
        _stateMachine = new PlayerStateMachine();
        _playerStamina = GetComponent<PlayerStamina>();
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsPlaying()) return;
        
        CheckGround();
        CheckWall();
        UpdateState();

        UpdateJumpBuffer();
        HandleWallJumpLock();
        HandleDash();
        HandleJump();

        TryDash();
    }

    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.IsPlaying())
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        
        HandleMovement();
        ApplyBetterGravity();
        HandleWallSlide();
    }

    // --------------------
    // STATE MACHINE
    // --------------------
    private void UpdateState()
    {
        if (_isDashing)
        {
            _stateMachine.ChangeState(PlayerState.Dashing);
        }
        else if (_isWallSliding)
        {
            _stateMachine.ChangeState(PlayerState.WallSliding);
        }
        else if (!_isGrounded)
        {
            _stateMachine.ChangeState(PlayerState.Airborne);
        }
        else
        {
            _stateMachine.ChangeState(PlayerState.Grounded);
        }
    }

    // --------------------
    // MOVEMENT
    // --------------------
    private void HandleMovement()
    {
        if (_isWallJumping || _isDashing) return;

        float currentSpeed = maxSpeed;

        if (_input.SprintHeld &&
            _input.MoveInput.x != 0 &&
            _playerStamina.CanSprint)
        {
            currentSpeed *= sprintMultiplier;
        }

        float targetSpeed = _input.MoveInput.x * currentSpeed;
        
        float speedDiff = targetSpeed - _rb.linearVelocityX;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        float movement = speedDiff * accelRate;

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocityX + movement * Time.fixedDeltaTime,
            _rb.linearVelocityY
        );
    }

    // --------------------
    // DASH
    // --------------------
    private void TryDash()
    {
        if (_input.DashPressed && CanDash())
        {
            StartDash();
        }
    }
    
    private bool CanDash()
    {
        if (_playerStamina == null)
            return false;

        if (_playerStamina.CurrentStamina < dashStaminaCost)
            return false;

        return true;
    }

    private void StartDash()
    {
        _isDashing = true;
        _dashTimeCounter = dashDuration;

        float direction = Mathf.Sign(_input.MoveInput.x);
        if (direction == 0) direction = transform.localScale.x;

        _playerStamina.DrainStamina(dashStaminaCost);
        _rb.linearVelocity = new Vector2(direction * dashForce, 0f);
    }

    private void HandleDash()
    {
        if (_isDashing)
        {
            _dashTimeCounter -= Time.deltaTime;

            if (_dashTimeCounter <= 0f)
            {
                _isDashing = false;
            }
        }
    }

    // --------------------
    // JUMP
    // --------------------
    private void HandleJump()
    {
        if (_stateMachine.CurrentState == PlayerState.Dashing) return;

        // WALL JUMP
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

        // SALTO BASE
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

        // CORTE
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
    // WALL LOCK
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

        if (touchingWallNow && !_wasTouchingWall && !_isGrounded)
        {
            _extraJumpsRemaining = maxExtraJumps;
        }

        _isTouchingWall = touchingWallNow;
        _wasTouchingWall = touchingWallNow;
    }

    // --------------------
    // BUFFER
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
        if (_rb.linearVelocityY < 0 && !_isDashing)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                  (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // --------------------
    // GROUND
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
            _coyoteTimeCounter = coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;

        _wasGrounded = _isGrounded;
    }
}