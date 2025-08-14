using System.Collections;
using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHitable
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected EnemyState state;
    [SerializeField] protected LayerMask groundLayerMask;
    protected PlayerController player;
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Get layermasks
        playerLayerMask = LayerMask.GetMask("Player");
    }

    protected virtual void Start()
    {
        SelectState();
        player = FindAnyObjectByType<PlayerController>();
    }
    protected abstract void SelectState();
    protected abstract void EnterState();
    protected abstract void UpdateState();

    protected private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canAttack)
        {
            Debug.Log("Da bat duoc va cham voi " + other.name);
            player.TakeDamage(1);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!getHit && hp >= dmg)    // Only takes damage if enemy can be hit
        {
            hp -= dmg;
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