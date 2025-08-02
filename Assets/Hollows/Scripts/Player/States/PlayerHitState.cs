using System.Collections;
using UnityEngine;

public class PlayerHitState : State
{
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocity = new Vector2(-3f * playerMovement.transform.localScale.x, 10f);
        StartCoroutine(WaitForCanBeHitAgain());
    }

    IEnumerator WaitForCanBeHitAgain()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement.canBeHit = true;
        ExitState();
    }
}
