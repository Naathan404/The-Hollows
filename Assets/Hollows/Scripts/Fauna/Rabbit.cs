using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    [Header("Behaviour Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float minBreakTime;
    [SerializeField] private float maxBreakTime;
    RaycastHit2D checkWall;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask wallLayerMask;

    private float idleTimer, walkTimer;

    private Animator animator;

    enum State
    {
        Idle, 
        Walk
    }
    State state;

    void Start()
    {
        animator = GetComponent<Animator>();
        state = State.Idle;
        EnterState();
    }

    void Update()
    {
        UpdateState();
    }

    private void EnterState()
    {
        switch (state)
        {
            case State.Idle:
                EnterIdle();
                break;
            case State.Walk:
                EnterWalk();
                break;
        }
    }

    private void UpdateState()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Walk:
                UpdateWalk();
                break;
        }
    }

    private void EnterIdle()
    {
        idleTimer = Random.Range(minBreakTime, maxBreakTime);
        animator.Play("rabbitIdle");
    }

    private void EnterWalk()
    {
        animator.Play("rabbitWalk");
        walkTimer = Random.Range(minBreakTime, maxBreakTime);
    }

    private void UpdateIdle()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            state = State.Walk;
            EnterState();
        }
    }

    private void UpdateWalk()
    {
        checkWall = Physics2D.Raycast(this.transform.position, new Vector2(this.transform.localScale.x, 0f), checkDistance, wallLayerMask);
        if (checkWall)
        {
            this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
        }
        walkTimer -= Time.deltaTime;
        if (walkTimer <= 0f)
        {
            state = State.Idle;
            EnterState();
        }
        this.transform.position += new Vector3(this.transform.localScale.x, 0f) * speed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(this.transform.localScale.x * checkDistance, 0f));
    }
}
