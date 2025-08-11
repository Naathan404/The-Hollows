using System.Collections;
using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHitable
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected EnemyState state;
    protected PlayerMovement player;
    protected LayerMask playerLayerMask;
    protected LayerMask groundLayerMask;
    protected Vector2 dir;
    protected bool isStateComplete = false;
    protected bool getHit = false;
    protected bool canAttack = true;

    /// References
    protected Animator animator;
    protected Rigidbody2D rb;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindAnyObjectByType<PlayerMovement>();

        // Get layermasks
        groundLayerMask = LayerMask.GetMask("Ground");
        playerLayerMask = LayerMask.GetMask("Player");
    }

    protected virtual void Start()
    {
        SelectState();
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
public enum EnemyState
{
    Idle,
    Patrol,
    Attack,
    Hit,
    Death
}