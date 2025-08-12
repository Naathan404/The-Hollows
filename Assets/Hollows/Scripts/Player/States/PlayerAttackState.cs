using System.Collections;
using UnityEngine;

public class PlayerAttackState : State
{
    [SerializeField] private EffectPooler attackEffectPool;
    public override void EnterState()
    {
        base.EnterState();
        rb.linearVelocityY = 0f;
        GameObject slash = attackEffectPool.GetObject();
        slash.transform.position = transform.position + (playerController.IsFacingRight() ? Vector3.right : Vector3.left) * 1.2f;
        slash.transform.localScale = new Vector2(playerController.IsFacingRight() ? 1f : -1f, slash.transform.localScale.y);
        StartCoroutine(ExitAttack(0.2f, slash));
    }
    public override void UpdateState()
    {

        if (Input.GetKeyDown(KeyCode.W))
            ExitState();
    }

    IEnumerator ExitAttack(float duration, GameObject slash)
    {
        yield return new WaitForSeconds(duration);
        slash.SetActive(false);
        ExitState();
    }
}
