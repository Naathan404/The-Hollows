using System.Collections;
using UnityEngine;

public class PlayerHitState : State
{
    [SerializeField] private float knockBack;
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocity = new Vector2(-knockBack * playerController.transform.localScale.x / playerController.dash.dash, 10f);
        UIManager.Instance.SubtractHeart();
        StartCoroutine(WaitForCanBeHitAgain());
    }

    IEnumerator WaitForCanBeHitAgain()
    {
        yield return new WaitForSeconds(0.5f);
        if (playerController.GetHP() <= 0)
        {
            playerController.state = playerController.death;
            playerController.state.EnterState();
        }
        else
        {
            playerController.canBeHit = true;
            ExitState();
        }
        
    }
}
