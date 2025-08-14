using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private int hp;
    // States controller
    [Header("States References")]
    public PlayerIdleState idle;
    public PlayerRunState run;
    public PlayerJumpState jump;
    public PlayerFallState fall;
    public PlayerDashState dash;
    public PlayerDoubleJumpState doubleJump;
    public PlayerAttackState attack;
    public PlayerHitState hit;
    public PlayerPushState push;
    public State state;

    [Header("Ground Check Settings")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundMask;
    public bool isGrounded { get; private set; }

    [Header("Jump Settings")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteCounter;
    [SerializeField] private float dashCoolDown;
    [HideInInspector] public bool canBeHit = true;
    [HideInInspector] public bool canJumpTheSecondTime = false;
    private float dashCounter;
    private bool isFacingRight = true;

    [Header("Raycast ")]
    public RaycastHit2D hitPushableObject;
    [SerializeField] private LayerMask pushableObjLayerMask;
    [SerializeField] private float rayLen;

    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private CameraFollowObj cameraFollowObj;

    /// <summary>
    /// Movement variables
    /// </summary>
    [HideInInspector] public float moveDirection { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraFollowObj = GameObject.Find("CamFollowObj").GetComponent<CameraFollowObj>();

        // Setup states
        idle.Setup(animator, rb, this);
        run.Setup(animator, rb, this);
        jump.Setup(animator, rb, this);
        fall.Setup(animator, rb, this);
        dash.Setup(animator, rb, this);
        doubleJump.Setup(animator, rb, this);
        attack.Setup(animator, rb, this);
        hit.Setup(animator, rb, this);
        push.Setup(animator, rb, this);

        // Default state is idle
        state = idle;
        state.EnterState();
        dashCounter = dashCoolDown;
    }

    private void Update()
    {
        //Debug.Log($"Velocity X: {rb.linearVelocity.x}");
        // Get input
        moveDirection = Input.GetAxisRaw("Horizontal");

        CheckGround();
        HandleJump();
        HandleDash();
        HandleAttack();
        HandlePush();

        dashCounter -= Time.deltaTime;

        /// Change state
        if (state.isComplete)
        {
            SelectState();
        }
        state.UpdateState();
    }

    private void FixedUpdate()
    {
        Move();
        Flip();
        ApplyFriction();
    }
    // Select new state to enter
    private void SelectState()
    {
        if (isGrounded)
        {
            if (moveDirection == 0) state = idle;
            else state = run;
        }
        else
        {
            if (rb.linearVelocity.y > 0f)
            {
                state = jump;
            }
            else state = fall;
        }
        state.EnterState();
    }
    // Move with accelaration
    private void Move()
    {
        float increament = moveDirection * run.accelaration;
        float newSpeed = Mathf.Clamp(rb.linearVelocity.x + increament, -run.moveSpeed, run.moveSpeed);
        rb.linearVelocity = new Vector2(newSpeed * dash.dash, rb.linearVelocity.y);
    }
    // Flip player
    private void Flip()
    {
        if (moveDirection > 0 && !isFacingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            isFacingRight = true;
            cameraFollowObj.TurnAround();
        }
        else if (moveDirection < 0 && isFacingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            isFacingRight = false;
            cameraFollowObj.TurnAround();
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || coyoteCounter > 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.jumpForce).normalized * jump.jumpForce;
        }
        else if (!Input.GetButton("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.8f);
        }
    }

    private void HandleDash()
    {
        if (Input.GetButtonDown("Dash") && (moveDirection != 0 || !isGrounded) && dashCounter <= 0f)
        {
            state.ExitState();
            state = dash;
            dashCounter = dashCoolDown;
            state.EnterState();
        }
    }
    private void HandleAttack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            state.ExitState();
            state = attack;
            state.EnterState();
        }
    }

    private void HandlePush()
    {
        hitPushableObject = Physics2D.Raycast(
            this.transform.position,
            isFacingRight ? Vector2.right : Vector2.left,
            rayLen, pushableObjLayerMask);
        if (hitPushableObject)
        {
            if ((isFacingRight && moveDirection > 0) || (!isFacingRight && moveDirection < 0))
            {
                state.ExitState();
                state = push;
                state.EnterState();
            }
        }
    }

    private void ApplyFriction()
    {
        if (isGrounded && moveDirection == 0)
            rb.linearVelocity *= run.groundDecay;
    }

    // Ground check
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;
    }


    public void TakeDamage(int dmg)
    {
        if (!canBeHit) return;
        // Decrease hp
        hp -= dmg;
        Debug.Log($"Current HP: {hp}");
        // Enter state hit
        state.ExitState();
        state = hit;
        canBeHit = false;
        state.EnterState();
    }

    // Take damage if the player collide with a trap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }

    // Getters
    public Transform GetBottomPosTransform() => groundCheck.transform;
    public bool IsFacingRight() => isFacingRight;
    public Rigidbody2D GetRb2D() => rb;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 direc = isFacingRight ? Vector3.right : Vector2.left;
        Gizmos.DrawLine(this.transform.position, this.transform.position + direc * rayLen);
    }
}