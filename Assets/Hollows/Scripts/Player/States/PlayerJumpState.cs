using System.Collections;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

public class PlayerJumpState : State
{
    public float jumpForce;
    [SerializeField] private EffectPooler beforeJumpDustPool;
    [SerializeField] private Transform bottomTransform;
    public override void EnterState()
    {
        base.EnterState();

        GameObject dust = beforeJumpDustPool.GetObject();
        dust.transform.position = bottomTransform.position;
        beforeJumpDustPool.ReturnToPool(dust);

        StartCoroutine(WaitForTheSecondJump());
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
            StopAllCoroutines();
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    IEnumerator WaitForTheSecondJump()
    {
        yield return new WaitForSeconds(0.1f);
        playerMovement.canJumpTheSecondTime = true;
    }
}
