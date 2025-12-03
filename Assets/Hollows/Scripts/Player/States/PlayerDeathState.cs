using System.Collections;
using UnityEngine;

public class PlayerDeathState : State
{
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        playerController.canBeHit = false;
        StartCoroutine(ExitDeath());
    }

    IEnumerator ExitDeath()
    {
        yield return new WaitForSeconds(0.4f);
        playerController.gameObject.SetActive(false);
    }
}
