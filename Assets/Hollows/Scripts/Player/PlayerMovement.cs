using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// States controller
    /// </summary>
    [Header("States References")]
    public PlayerIdleState idle;
    public PlayerRunState run;
    public PlayerJumpState jump;
    public PlayerFallState fall;
    public PlayerDashState dash;
    public PlayerDoubleJumpState doubleJump;
    public PlayerAttackState attack;
    public PlayerHitState hit;
    public State state;

    [Header("Ground Check Settings")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundMask;
    public bool isGrounded { get; private set; }

    [Header("Jump Settings")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteCounter;
    [SerializeField] private float dashCoolDown;
    public bool canJumpTheSecondTime = false;
    private float dashCounter;
    public bool canBeHit = true;

    private bool isFacingRight = true;

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

        // Setup states
        idle.Setup(animator, rb, this);
        run.Setup(animator, rb, this);
        jump.Setup(animator, rb, this);
        fall.Setup(animator, rb, this);
        dash.Setup(animator, rb, this);
        doubleJump.Setup(animator, rb, this);
        attack.Setup(animator, rb, this);
        hit.Setup(animator, rb, this);

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

        if (isGrounded) canJumpTheSecondTime = false;
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

    private void Move()
    {
        float increament = moveDirection * run.accelaration;
        float newSpeed = Mathf.Clamp(rb.linearVelocity.x + increament, -run.moveSpeed, run.moveSpeed);
        rb.linearVelocity = new Vector2(newSpeed * dash.dash, rb.linearVelocity.y);
    }
    private void Flip()
    {
        /// Flip player
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

    public bool IsFacingRight() => isFacingRight;


    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || coyoteCounter > 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.jumpForce).normalized * jump.jumpForce;
        }
        else if (!Input.GetKey(KeyCode.W) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.8f);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (moveDirection != 0 || !isGrounded) && dashCounter <= 0f)
        {
            state.ExitState();
            state = dash;
            dashCounter = dashCoolDown;
            state.EnterState();
        }
    }

    private void ApplyFriction()
    {
        if (isGrounded && moveDirection == 0)
            rb.linearVelocity *= run.groundDecay;
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            state.ExitState();
            state = attack;
            state.EnterState();
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!canBeHit) return;
        state.ExitState();
        state = hit;
        canBeHit = false;
        state.EnterState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
        }
    }
}