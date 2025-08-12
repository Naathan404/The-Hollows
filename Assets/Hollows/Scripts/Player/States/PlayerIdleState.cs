using UnityEngine;

public class PlayerIdleState : State
{
    public override void UpdateState()
    {
        if (!playerController.isGrounded || playerController.moveDirection != 0)
            ExitState();
    }

}
