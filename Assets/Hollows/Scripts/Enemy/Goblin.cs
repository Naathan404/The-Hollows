using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : Enemy, IHitable
{
    [Header("Stats")]
    [SerializeField] private float maxWallRayDistance;
    [SerializeField] private float maxPlayerRayDistance;
    [SerializeField] private bool getHit = false;
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
        {
            animator.Play("GoblinHit");
            state = EnemyState.Hit;
            EnterHit();
        }
        else if (hp <= 0)
        {
            state = EnemyState.Death;
            EnterDeath();
        }
        else if (hitPlayer)
        {
            animator.Play("GoblinAttack");
            state = EnemyState.Attack;
            EnterAttack();
        }
        else
        {
            animator.Play("GoblinRun");
            state = EnemyState.Patrol;
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
            case EnemyState.Attack:
                UpdateAttack();
                break;
            case EnemyState.Hit:
                UpdateHit();
                break;
            case EnemyState.Death:
                UpdateDeath();
                break;
        }
    }

    private void UpdateHit()
    {

    }

    private void UpdateIdle()
    {

    }

    private void UpdatePatrol()
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        if (hitPlayer || getHit)
        {
            isStateComplete = true;
        }
    }

    private void UpdateAttack()
    {
        // rb.linearVelocity = Vector2.zero; // Stop movement during attack
        // StartCoroutine(WaitForDurationToComplete());
        // isStateComplete = true;
    }

    private void EnterAttack()
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(WaitForDurationToAttackAgain(0.5f));
    }

    private void EnterHit()
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(WaitForDurationToGetHitAgain(0.8f));
    }

    private void EnterDeath()
    {
        rb.linearVelocity = Vector2.zero;
        animator.Play("GoblinDeath");
        StartCoroutine(DeactivateObject(0.8f));
    }

    private void UpdateDeath()
    {
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
    }

    IEnumerator DeactivateObject(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        if (!getHit)
        {
            hp--;
            getHit = true;
        }
    }
}
