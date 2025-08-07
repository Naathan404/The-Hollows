using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : Enemy, IHitable
{
    [Header("Stats")]
    [SerializeField] private float maxWallRayDistance;
    [SerializeField] private float maxPlayerRayDistance;
    ///
    /// Raycasts
    private RaycastHit2D hitWall;
    private RaycastHit2D hitPlayer;

    private void Start()
    {
        SelectState();
    }

    private void Update()
    {
        dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        hitPlayer = Physics2D.Raycast(transform.position, dir, maxPlayerRayDistance, playerLayerMask);
        hitWall = Physics2D.Raycast(transform.position, dir, maxWallRayDistance, LayerMask.GetMask("Ground"));
        if (hitWall)
        {
            speed *= -1f;
            Debug.DrawRay(transform.position, dir * maxPlayerRayDistance, Color.red);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            Debug.DrawRay(transform.position, dir * maxPlayerRayDistance, Color.white);
        }

        if (hitPlayer)
            Debug.Log("Hit player");

        if (isStateComplete)
        {
            SelectState();
        }
        UpdateState();
    }

    protected override void SelectState()
    {
        isStateComplete = false;
        if (getHit)
            state = EnemyState.Hit;
        else if (hp <= 0)
            state = EnemyState.Death;
        else if (hitPlayer)
            state = EnemyState.Attack;
        else
            state = EnemyState.Patrol;
        EnterState();
    }

    protected override void EnterState()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                EnterPatrol();
                break;
            case EnemyState.Attack:
                EnterAttack();
                break;
            case EnemyState.Hit:
                EnterHit();
                break;
            case EnemyState.Death:
                EnterDeath();
                break;
            default:
                //EnterIdle();
                break;
        }
    }

    protected override void UpdateState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
        }
    }

    /// <summary>
    /// Enter state implementation
    /// </summary>
    private void EnterPatrol()
    {
        animator.Play("GoblinRun");
    }

    private void EnterAttack()
    {
        animator.Play("GoblinAttack");
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(WaitForDurationToAttackAgain(0.5f));
    }

    private void EnterHit()
    {
        canAttack = false;
        animator.Play("GoblinHit");
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(WaitForDurationToGetHitAgain(0.8f));
    }

    private void EnterDeath()
    {
        canAttack = false;
        rb.linearVelocity = Vector2.zero;
        animator.Play("GoblinDeath");
        StartCoroutine(DeactivateObject(0.6f));
    }

    IEnumerator WaitForDurationToAttackAgain(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStateComplete = true;
    }

    IEnumerator WaitForDurationToGetHitAgain(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStateComplete = true;
        getHit = false;
        canAttack = true;
    }

    IEnumerator DeactivateObject(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }


    private void UpdateIdle() { }

    private void UpdatePatrol()
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        if (hitPlayer || getHit)
        {
            isStateComplete = true;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!getHit)
        {
            hp--;
            getHit = true;
            SelectState();
        }
    }
}