using System.Collections;
using UnityEngine;

public class PlayerJumpState : State
{
    public float jumpForce;
    [SerializeField] private EffectPooler beforeJumpDustPool;
    [SerializeField] private Transform bottomTransform;
    public override void EnterState()
    {
        base.EnterState();
        playerMovement.canJumpTheSecondTime = true;

        GameObject dust = beforeJumpDustPool.GetObject();
        dust.transform.position = bottomTransform.position;
        beforeJumpDustPool.ReturnToPool(dust);
    }
    public override void UpdateState()
    {
        if (playerMovement.canJumpTheSecondTime && Input.GetKeyDown(KeyCode.W))
        {
            ExitState();
            playerMovement.state = playerMovement.doubleJump;
            playerMovement.state.EnterState();
        }
        else if (rb.linearVelocity.y <= 0)
        {
            ExitState();
        }
    }
}
