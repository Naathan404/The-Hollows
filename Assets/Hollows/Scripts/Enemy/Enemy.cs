using System.Collections;
using System.ComponentModel;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected EnemyState state;
    protected LayerMask playerLayerMask;
    protected Vector2 dir;
    protected bool isStateComplete = false;

    /// References
    protected Animator animator;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        playerLayerMask = LayerMask.GetMask("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void SelectState() { }
    protected virtual void UpdateState() { }

}
public enum EnemyState
{
    Idle,
    Patrol,
    Attack,
    Hit,
    Death
}