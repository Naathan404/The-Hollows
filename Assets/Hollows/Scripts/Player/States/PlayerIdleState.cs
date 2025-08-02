using UnityEngine;

public class PlayerIdleState : State
{
    public override void UpdateState()
    {
        if (!playerMovement.isGrounded || playerMovement.moveDirection != 0)
            ExitState();
    }

}
