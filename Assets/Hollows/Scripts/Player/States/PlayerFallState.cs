using System.Collections;
using UnityEngine;

public class PlayerFallState : State
{
    [SerializeField] private EffectPooler afterJumpDustPool;
    [SerializeField] private Transform bottomTransform;

    public override void UpdateState()
    {
        if (playerMovement.canJumpTheSecondTime && Input.GetKeyDown(KeyCode.W))
        {
            ExitState();
            playerMovement.state = playerMovement.doubleJump;
            playerMovement.state.EnterState();
        }
        if (playerMovement.isGrounded || rb.linearVelocity.y >= 0f)
        {
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        GameObject dust = afterJumpDustPool.GetObject();
        dust.transform.position = bottomTransform.position;
        afterJumpDustPool.ReturnToPool(dust);
    }
}
