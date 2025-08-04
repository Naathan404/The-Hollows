using System.Collections;
using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected EnemyState state;
    protected PlayerMovement player;
    protected LayerMask playerLayerMask;
    protected Vector2 dir;
    protected bool isStateComplete = false;
    protected bool getHit = false;
    protected bool canAttack = true;

    /// References
    protected Animator animator;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindAnyObjectByType<PlayerMovement>();
    }

    protected abstract void SelectState();
    protected abstract void EnterState();
    protected abstract void UpdateState();

    protected private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canAttack)
        {
            player.TakeDamage(1);
        }
    }

}
public enum EnemyState
{
    Idle,
    Patrol,
    Attack,
    Hit,
    Death
}