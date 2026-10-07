using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
/// <summary>
/// Coordinates player behavior and its Unity lifecycle.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Animator hashes are cached once to avoid repeated string lookups at runtime.
    private static readonly int AnimStateHash = Animator.StringToHash("AnimState");
    private static readonly int RollHash = Animator.StringToHash("Roll");
    private static readonly int AttackHash = Animator.StringToHash("Attack1");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int AirSpeedYHash = Animator.StringToHash("AirSpeedY");

    // Runtime dependencies cached during initialization.
    private Rigidbody2D _rb;
    private PlayerInputHandler _input;
    private PlayerStateMachine _stateMachine;
    private PlayerStamina _playerStamina;

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

    [Header("Attack")]
    private bool _isAttacking;

    [SerializeField]
    private Animator animator;

    #region Unity Lifecycle

    /// <summary>
    /// Caches the Rigidbody, input adapter, state machine, and stamina dependency required by the controller.
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInputHandler>();
        _stateMachine = new PlayerStateMachine();
        _playerStamina = GetComponent<PlayerStamina>();
    }

    /// <summary>
    /// Coordinates frame-based gameplay logic while the global game state allows player control.
    /// </summary>
    private void Update()
    {
        if (!IsGameplayActive())
            return;

        ProcessGameplayFrame();
    }

    /// <summary>
    /// Coordinates Rigidbody movement and traversal physics on Unity's fixed timestep.
    /// </summary>
    private void FixedUpdate()
    {
        if (!IsGameplayActive())
        {
            StopPhysicsMovement();
            return;
        }

        ProcessPhysicsFrame();
    }

    #endregion

    #region Frame Orchestration

    /// <summary>
    /// Coordinates frame-based gameplay systems. Keeping orchestration here makes
    /// Unity's Update callback easy to scan while each subsystem owns one concern.
    /// </summary>
    private void ProcessGameplayFrame()
    {
        RefreshEnvironmentState();
        UpdateAbilityTimers();
        ProcessActionInput();
    }

    /// <summary>
    /// Coordinates all Rigidbody changes from Unity's fixed-timestep loop.
    /// </summary>
    private void ProcessPhysicsFrame()
    {
        HandleMovement();
        ApplyBetterGravity();
        HandleWallSlide();
    }

    /// <summary>
    /// Refreshes ground and wall contacts before deriving the current player state.
    /// </summary>
    private void RefreshEnvironmentState()
    {
        // State selection depends on the latest ground and wall observations.
        CheckGround();
        CheckWall();
        UpdateState();
    }

    /// <summary>
    /// Advances buffered input and active traversal ability timers.
    /// </summary>
    private void UpdateAbilityTimers()
    {
        UpdateJumpBuffer();
        HandleWallJumpLock();
        HandleDash();
    }

    /// <summary>
    /// Processes jump, dash, and attack requests in their intentional priority order.
    /// </summary>
    private void ProcessActionInput()
    {
        // Preserve the original action order because abilities share velocity and state.
        HandleJump();
        TryDash();
        HandleAttack();
    }

    /// <summary>
    /// Clears residual Rigidbody velocity while gameplay is suspended.
    /// </summary>
    private void StopPhysicsMovement()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    #endregion

    #region Game State

    /// <summary>
    /// Returns whether the global game state currently permits player gameplay.
    /// </summary>
    private static bool IsGameplayActive()
    {
        return GameStateManager.Instance != null && GameStateManager.Instance.IsPlaying();
    }

    /// <summary>
    /// Selects the highest-priority locomotion state from the current runtime flags.
    /// </summary>
    private void UpdateState()
    {
        // Transient abilities have priority over general locomotion states.
        if (_isDashing)
            _stateMachine.ChangeState(PlayerState.Dashing);
        else if (_isWallSliding)
            _stateMachine.ChangeState(PlayerState.WallSliding);
        else if (!_isGrounded)
            _stateMachine.ChangeState(PlayerState.Airborne);
        else if (_isAttacking)
            _stateMachine.ChangeState(PlayerState.Attacking);
        else
            _stateMachine.ChangeState(PlayerState.Grounded);
    }

    #endregion

    #region Movement

    /// <summary>
    /// Calculates accelerated horizontal velocity and refreshes movement presentation.
    /// </summary>
    private void HandleMovement()
    {
        // Dash and wall-jump impulses temporarily own horizontal velocity.
        if (_isWallJumping || _isDashing)
            return;

        float currentSpeed = maxSpeed;

        if (_input.SprintHeld &&
            _input.MoveInput.x != 0 &&
            _playerStamina != null &&
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

        UpdateMovementVisuals();
    }

    /// <summary>
    /// Updates locomotion animation and facing direction from horizontal input.
    /// </summary>
    private void UpdateMovementVisuals()
    {
        if (_input.MoveInput.x != 0)
        {
            animator.SetInteger(AnimStateHash, 1);
            FaceMovementDirection();
        }
        else
        {
            animator.SetInteger(AnimStateHash, 0);
        }
    }

    /// <summary>
    /// Rotates the character to face the current horizontal movement direction.
    /// </summary>
    private void FaceMovementDirection()
    {
        if (_input.MoveInput.x < 0)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    #endregion

    #region Environment Detection

    /// <summary>
    /// Detects ground contact and manages landing, coyote time, and grounded animation state.
    /// </summary>
    private void CheckGround()
    {
        bool groundedNow = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // Landing restores the base jump and all configured extra jumps.
        if (groundedNow && !_wasGrounded)
        {
            _extraJumpsRemaining = maxExtraJumps;
            _hasJumped = false;
        }

        _isGrounded = groundedNow;

        if (_isGrounded)
        {
            // Refresh coyote time continuously while the player is grounded.
            _coyoteTimeCounter = coyoteTime;
            animator.SetBool(GroundedHash, true);
            animator.SetFloat(AirSpeedYHash, 0f);
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            animator.SetBool(GroundedHash, false);
        }

        _wasGrounded = _isGrounded;
    }

    /// <summary>
    /// Detects adjacent walls and refreshes wall direction and aerial jump resources.
    /// </summary>
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

        if (hitRight)
            _wallDir = 1;
        else if (hitLeft)
            _wallDir = -1;
        else
            _wallDir = 0;

        // First contact with a wall refreshes aerial jump resources.
        if (touchingWallNow && !_wasTouchingWall && !_isGrounded)
            _extraJumpsRemaining = maxExtraJumps;

        _isTouchingWall = touchingWallNow;
        _wasTouchingWall = touchingWallNow;
    }

    #endregion

    #region Jump

    /// <summary>
    /// Maintains the short input window used to accept an early jump request.
    /// </summary>
    private void UpdateJumpBuffer()
    {
        // Buffering accepts a jump pressed shortly before a valid jump opportunity.
        if (_input.JumpPressed)
            _jumpBufferCounter = jumpBufferTime;
        else
            _jumpBufferCounter -= Time.deltaTime;
    }

    /// <summary>
    /// Coordinates wall jump, standard jump, and variable jump-height behavior.
    /// </summary>
    private void HandleJump()
    {
        if (_stateMachine.CurrentState == PlayerState.Dashing)
            return;

        if (TryWallJump())
            return;

        TryStandardJump();
        ApplyVariableJumpHeight();
    }

    /// <summary>
    /// Attempts a wall jump and locks regular movement while its impulse is active.
    /// </summary>
    private bool TryWallJump()
    {
        if (_jumpBufferCounter <= 0f || !_isWallSliding)
            return false;

        _rb.linearVelocity = new Vector2(
            -_wallDir * wallJumpForceX,
            wallJumpForceY
        );

        _isWallJumping = true;
        _wallJumpLockCounter = wallJumpLockTime;
        _jumpBufferCounter = 0f;
        return true;
    }

    /// <summary>
    /// Attempts a coyote-time jump or consumes an available extra jump.
    /// </summary>
    private void TryStandardJump()
    {
        // Coyote time allows the base jump shortly after leaving a platform.
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f && !_hasJumped)
        {
            Jump();
            _hasJumped = true;
            _coyoteTimeCounter = 0f;
        }
        else if (_jumpBufferCounter > 0f && _extraJumpsRemaining > 0 && !_isGrounded)
        {
            Jump();
            _extraJumpsRemaining--;
        }
    }

    /// <summary>
    /// Shortens upward velocity when the jump input is released early.
    /// </summary>
    private void ApplyVariableJumpHeight()
    {
        // Releasing jump during ascent produces a shorter, more responsive jump.
        if (_input.JumpHeld || _rb.linearVelocityY <= 0)
            return;

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocityX,
            _rb.linearVelocityY * 0.5f
        );

        animator.SetFloat(AirSpeedYHash, -1f);
    }

    /// <summary>
    /// Applies the configured vertical jump impulse and animation trigger.
    /// </summary>
    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
        _jumpBufferCounter = 0f;
        animator.SetTrigger(JumpHash);
    }

    /// <summary>
    /// Applies additional gravity while falling to sharpen the descent.
    /// </summary>
    private void ApplyBetterGravity()
    {
        // Additional downward gravity sharpens the fall without changing the ascent.
        if (_rb.linearVelocityY < 0 && !_isDashing)
        {
            _rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                  (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Wall Movement

    /// <summary>
    /// Limits downward velocity while the player pushes toward a contacted wall.
    /// </summary>
    private void HandleWallSlide()
    {
        float inputDir = _input.MoveInput.x;
        bool pushingToWall = _isTouchingWall && inputDir == _wallDir;

        if (pushingToWall && !_isGrounded && _rb.linearVelocityY < 0)
        {
            _isWallSliding = true;
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, -wallSlideSpeed);

            // Reserved for a future dedicated wall-slide animation state.
            /*animator.SetBool("WallSlide", true);
            animator.SetFloat("AirSpeedY", -1f);*/
        }
        else
        {
            _isWallSliding = false;
            //animator.SetBool("WallSlide", false);
        }
    }

    /// <summary>
    /// Releases the temporary movement lock after a wall jump.
    /// </summary>
    private void HandleWallJumpLock()
    {
        if (!_isWallJumping)
            return;

        // Prevent standard movement from immediately cancelling the wall-jump impulse.
        _wallJumpLockCounter -= Time.deltaTime;

        if (_wallJumpLockCounter <= 0f)
            _isWallJumping = false;
    }

    #endregion

    #region Dash

    /// <summary>
    /// Starts a dash when input and stamina requirements are satisfied.
    /// </summary>
    private void TryDash()
    {
        if (!_input.DashPressed || !CanDash())
            return;

        StartDash();
    }

    /// <summary>
    /// Returns whether the player has enough stamina to start a dash.
    /// </summary>
    private bool CanDash()
    {
        return _playerStamina != null &&
               _playerStamina.CurrentStamina >= dashStaminaCost;
    }

    /// <summary>
    /// Consumes stamina and applies the configured dash impulse and animation.
    /// </summary>
    private void StartDash()
    {
        _isDashing = true;
        _dashTimeCounter = dashDuration;

        // Use movement input first; otherwise dash in the facing direction.
        float direction = Mathf.Sign(_input.MoveInput.x);
        if (Mathf.Approximately(direction, 0f))
            direction = Mathf.Sign(transform.right.x);

        _playerStamina.DrainStamina(dashStaminaCost);
        _rb.linearVelocity = new Vector2(direction * dashForce, 0f);
        animator.SetTrigger(RollHash);
    }

    /// <summary>
    /// Advances the active dash timer and ends the dash when its duration expires.
    /// </summary>
    private void HandleDash()
    {
        if (!_isDashing)
            return;

        _dashTimeCounter -= Time.deltaTime;

        if (_dashTimeCounter <= 0f)
            _isDashing = false;
    }

    #endregion

    #region Combat

    /// <summary>
    /// Starts an attack when input is received and no attack is already active.
    /// </summary>
    private void HandleAttack()
    {
        if (!_input.AttackPressed || _isAttacking)
            return;

        _isAttacking = true;
        animator.SetTrigger(AttackHash);
    }

    /// <summary>
    /// Releases the attack lock. Intended to be called by an animation event.
    /// </summary>
    public void EndAttack()
    {
        _isAttacking = false;
    }

    #endregion
}
