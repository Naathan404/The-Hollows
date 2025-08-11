using System;
using System.Collections;
using UnityEngine;

public class Goblin : Enemy
{
    [Header("Stats")]
    [SerializeField] private float maxWallRayDistance;
    [SerializeField] private float maxPlayerRayDistance;
    [SerializeField] private float edgeRayDistance;
    ///
    /// Raycasts
    private RaycastHit2D hitWall;
    private RaycastHit2D hitPlayer;
    private RaycastHit2D hitGround;

    private void Update()
    {
        dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        hitPlayer = Physics2D.Raycast(transform.position, dir, maxPlayerRayDistance, playerLayerMask);
        hitWall = Physics2D.Raycast(transform.position, dir, maxWallRayDistance, groundLayerMask);
        hitGround = Physics2D.Raycast(transform.position + new Vector3(edgeRayDistance * this.transform.localScale.x, 0), -Vector2.up, 1f, groundLayerMask);

        // Turn around if there was a wall in front of enemy
        if (hitWall || !hitGround)
        {
            speed *= -1f;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }

        // Draw ray on scene
        Debug.DrawRay(transform.position, dir * maxPlayerRayDistance, Color.red);
        Debug.DrawRay(transform.position + new Vector3(edgeRayDistance * this.transform.localScale.x, 0), -Vector2.up * 1f, Color.yellow);

        // Select new state when current state was complete
        if (isStateComplete)
            SelectState();

        // Update state 
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
        StartCoroutine(WaitForDurationToGetHitAgain());
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

    IEnumerator WaitForDurationToGetHitAgain()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
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
}