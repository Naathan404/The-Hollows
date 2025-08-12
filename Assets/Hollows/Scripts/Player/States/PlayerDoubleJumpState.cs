using UnityEngine;

public class PlayerDoubleJumpState : State
{
    public float doubleJumpForce;
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        playerController.canJumpTheSecondTime = false;
    }

    public override void UpdateState()
    {
        if (rb.linearVelocity.y <= 0)
        {
            ExitState();
        }
    }
}
