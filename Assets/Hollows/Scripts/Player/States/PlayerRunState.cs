using UnityEngine;

public class PlayerRunState : State
{
    public float moveSpeed;
    public float accelaration;
    [Range(0f, 1f)] public float groundDecay;
    public override void UpdateState()
    {
        if (!playerController.isGrounded || Mathf.Abs(rb.linearVelocity.x) < 1f)
        {
            ExitState();
        }
    }

}
