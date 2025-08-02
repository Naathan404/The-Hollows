using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }
    public AnimationClip anim;

    /// <summary>
    /// Required component references
    /// </summary>
    protected Animator animator;
    protected Rigidbody2D rb;
    protected PlayerMovement playerMovement;

    public virtual void EnterState()
    {
        animator.Play(anim.name);
        isComplete = false;
        //Debug.Log(name);
    }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState()
    {
        isComplete = true;
    }

    public void Setup(Animator animator, Rigidbody2D rb, PlayerMovement playerMovement)
    {
        this.animator = animator;
        this.rb = rb;
        this.playerMovement = playerMovement;
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerMovement.canBeHit)
        {
            playerMovement.state.ExitState();
            playerMovement.state = playerMovement.hit;
            playerMovement.canBeHit = false;
            playerMovement.state.EnterState();
        }
    }
}
