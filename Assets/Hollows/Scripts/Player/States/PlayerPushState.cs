using UnityEngine;

public class PlayerPushState : State
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        if (playerController.moveDirection == 0 || !playerController.hitPushableObject)
        {
            ExitState();
        }
    }
}
