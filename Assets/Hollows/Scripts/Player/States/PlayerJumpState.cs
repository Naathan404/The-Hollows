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
        if (playerController.canJumpTheSecondTime && Input.GetButtonDown("Jump"))
        {
            ExitState();
            playerController.state = playerController.doubleJump;
            playerController.state.EnterState();
        }
        else if (rb.linearVelocity.y <= 0)
        {
            StopAllCoroutines();
            ExitState();
        }
    }

    IEnumerator WaitForTheSecondJump()
    {
        yield return new WaitForSeconds(0.1f);
        playerController.canJumpTheSecondTime = true;
    }
}
