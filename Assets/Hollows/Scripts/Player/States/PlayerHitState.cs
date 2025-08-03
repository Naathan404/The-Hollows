using System.Collections;
using UnityEngine;

public class PlayerHitState : State
{
    [SerializeField] private float knockBack;
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocity = new Vector2(-knockBack * playerMovement.transform.localScale.x / playerMovement.dash.dash, 10f);
        StartCoroutine(WaitForCanBeHitAgain());
    }

    IEnumerator WaitForCanBeHitAgain()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement.canBeHit = true;
        ExitState();
    }
}
