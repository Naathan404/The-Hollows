using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Slime : Enemy
{
    RaycastHit2D detectPlayerFromTheLeft;
    RaycastHit2D detectPlayerFromTheRight;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoolDown;
    [SerializeField] private EffectPooler fallDustPooler;
    private float timer = 0f;

    protected override void Start()
    {
        base.Start();
        fallDustPooler = GameObject.Find("AfterJumpDustPool").GetComponent<EffectPooler>();
    }
    private void Update()
    {
        detectPlayerFromTheLeft = Physics2D.Raycast(this.transform.position + new Vector3(0, 0.5f), Vector2.left, detectionDistance, playerLayerMask);
        detectPlayerFromTheRight = Physics2D.Raycast(this.transform.position + new Vector3(0, 0.5f), Vector2.right, detectionDistance, playerLayerMask);
        if (detectPlayerFromTheRight)
            this.transform.localScale = new Vector3(-1f, 1f);
        if (detectPlayerFromTheLeft)
            this.transform.localScale = new Vector3(1f, 1f);
        // Draw rays
        Debug.DrawRay(this.transform.position + new Vector3(0, 0.5f), Vector2.left * detectionDistance, Color.red);
        Debug.DrawRay(this.transform.position + new Vector3(0, 0.5f), Vector2.right * detectionDistance, Color.red);

        if (isStateComplete)
            SelectState();

        UpdateState();
    }
    protected override void EnterState()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                EnterPatrol();
                break;
            case EnemyState.Hit:
                EnterHit();
                break;
            case EnemyState.Death:
                EnterDeath();
                break;
            default:
                EnterIdle();
                break;
        }
    }

    protected override void SelectState()
    {
        isStateComplete = false;
        if (getHit)
            state = EnemyState.Hit;
        else if (hp <= 0)
            state = EnemyState.Death;
        else if (detectPlayerFromTheLeft || detectPlayerFromTheRight)
            state = EnemyState.Patrol;
        else
            state = EnemyState.Idle;
        EnterState();
    }

    protected override void UpdateState()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
            default:
                UpdateIdle();
                break;
        }
    }

    private void EnterIdle()
    {
        animator.Play("slimeIdle");
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void EnterPatrol()
    {
        StartCoroutine(Jump());
        timer = jumpCoolDown;
    }

    private void EnterHit()
    {
        animator.Play("SlimeHit");
        canAttack = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        StartCoroutine(ExitHit());
    }
    private void EnterDeath()
    {
        animator.Play("SlimeDeath");
        canAttack = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        StartCoroutine(ExitDeath());
    }
    IEnumerator ExitHit()
    {
        yield return new WaitForSeconds(0.4f);
        isStateComplete = true;
        getHit = false;
        canAttack = true;
        StopAllCoroutines();
    }
    IEnumerator ExitDeath()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void UpdateIdle()
    {
        if (detectPlayerFromTheLeft || detectPlayerFromTheRight)
            isStateComplete = true;
    }
    private void UpdatePatrol()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            StartCoroutine(Jump());
            timer = jumpCoolDown;
        }
    }

    IEnumerator Jump()
    {
        animator.Play("SlimeWalk");
        // start jump
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(new Vector2(jumpForce * -this.transform.localScale.x, 0f));
        // end jump
        yield return new WaitForSeconds(0.8f);
        animator.Play("slimeIdle");
        rb.linearVelocity = Vector2.zero;
        if (!detectPlayerFromTheLeft && !detectPlayerFromTheRight)
            isStateComplete = true;

        // some effects to make this slime is more fk cool
        GameObject dust1 = fallDustPooler.GetObject();
        dust1.transform.position = this.transform.position;
        fallDustPooler.ReturnToPool(dust1);
    }
}
