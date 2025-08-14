using System.Collections;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

public class Bomber : Enemy
{
    RaycastHit2D detectPlayerFromTheLeft;
    RaycastHit2D detectPlayerFromTheRight;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float timer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private ObjectPooler bombPool;
    private bool attackable = true;

    protected override void Start()
    {
        base.Start();
        timer = attackCoolDown;
    }

    private void Update()
    {
        detectPlayerFromTheLeft = Physics2D.Raycast(this.transform.position, Vector2.left, detectionDistance, playerLayerMask);
        detectPlayerFromTheRight = Physics2D.Raycast(this.transform.position, Vector2.right, detectionDistance, playerLayerMask);

        if (detectPlayerFromTheLeft)
        {
            this.transform.localScale = new Vector3(-1f, 1f);
        }
        if (detectPlayerFromTheRight)
        {
            this.transform.localScale = new Vector3(1f, 1f);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = attackCoolDown;
            attackable = true;
        }

        if (isStateComplete)
                SelectState();
        UpdateState();
    }
    protected override void EnterState()
    {
        switch (state)
        {
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
        else if ((detectPlayerFromTheLeft || detectPlayerFromTheRight) && attackable)
            state = EnemyState.Attack;
        else
            state = EnemyState.Idle;
        EnterState();
    }

    protected override void UpdateState()
    {
        switch (state)
        {
            case EnemyState.Attack:
                //UpdateAttack();
                break;
            default:
                UpdateIdle();
                break;
        }
    }

    private void EnterIdle()
    {
        animator.Play("BomberIdle");
    }
    private void EnterAttack()
    {
        animator.Play("BomberAttack");
        attackable = false;
        StartCoroutine(ExitAttack());
    }
    private void EnterHit()
    {
        animator.Play("BomberHit");
    }
    private void EnterDeath()
    {
        animator.Play("BomberDeath");
    }

    private void UpdateIdle()
    {
        if ((detectPlayerFromTheLeft || detectPlayerFromTheRight) && attackable)
        {
            isStateComplete = true;
        }
    }

    IEnumerator ExitAttack()
    {
        yield return new WaitForSeconds(0.4f);
        GameObject bomb = bombPool.GetObject();
        bomb.SetActive(true);
        bomb.GetComponent<Bomb>().Throw(startPoint, player.transform, this.transform.localScale.x);
        isStateComplete = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.left * detectionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.right * detectionDistance);
    }
}
