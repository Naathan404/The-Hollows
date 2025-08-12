using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDashState : State
{
    [Header("Dash Settings")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private EffectPooler beforeJumpDustPool;
    [SerializeField] private Transform bottomTransform;
    [Header("Ghost Settings")]
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private float ghostCoolDown;
    [SerializeField] private int maxGhostCount;
    [SerializeField] private float ghostGap;
    [SerializeField] private float ghostDuration;
    private Vector2 firstPos;
    private float ghostCounter;
    private List<GameObject> ghosts = new List<GameObject>();
    public float dash;

    public override void EnterState()
    {
        base.EnterState();
        dash = dashForce;
        StartCoroutine(ExitDash(dashDuration));
        GameObject dust = beforeJumpDustPool.GetObject();
        dust.transform.position = bottomTransform.position;
        beforeJumpDustPool.ReturnToPool(dust);
        firstPos = playerController.transform.position;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        ghostCounter -= Time.deltaTime;
        if (ghostCounter <= 0f && ghosts.Count < maxGhostCount)
        {
            ghostCounter = ghostCoolDown;
            CreateGhost();
        }
    }

    private void CreateGhost()
    {
        ghostGap = maxGhostCount > 1 ? Mathf.Abs(playerController.transform.position.x - firstPos.x) / (maxGhostCount - 1) : 0f;
        GameObject ghost = objectPooler.GetObject();
        ghost.transform.localScale = playerController.transform.localScale;
        ghost.GetComponent<SpriteRenderer>().sortingOrder = ghosts.Count + 1;
        float gap = playerController.IsFacingRight() ? ghostGap * ghosts.Count : -ghosts.Count * ghostGap;
        ghost.transform.position = new Vector2(playerController.transform.position.x - gap, playerController.transform.position.y);
        ghosts.Add(ghost);
        StartCoroutine(FadeOut(ghost, ghostDuration));
    }

    IEnumerator ExitDash(float duration)
    {
        yield return new WaitForSeconds(duration);
        dash = 1f;
        ghosts.Clear();
        ExitState();
    }

    IEnumerator FadeOut(GameObject pghost, float duration)
    {
        float elapsedTime = 0f;
        SpriteRenderer spriteRenderer = pghost.GetComponent<SpriteRenderer>();
        Color initialColor = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        objectPooler.ReturnToPool(pghost);
    }
}
