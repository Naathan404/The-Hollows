using System.Collections;
using UnityEngine;

public class PlayerFallState : State
{
    [SerializeField] private EffectPooler afterJumpDustPool;
    [SerializeField] private Transform bottomTransform;

    public override void UpdateState()
    {
        if (playerController.canJumpTheSecondTime && Input.GetKeyDown(KeyCode.W))
        {
            ExitState();
            playerController.state = playerController.doubleJump;
            playerController.state.EnterState();
        }
        if (playerController.isGrounded || rb.linearVelocity.y >= 0f)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        GameObject dust = afterJumpDustPool.GetObject();
        dust.transform.position = bottomTransform.position;
        playerController.canJumpTheSecondTime = false;
        afterJumpDustPool.ReturnToPool(dust);
    }
}
