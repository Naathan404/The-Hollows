using System;
using System.Collections;
using UnityEngine;

public class EffectPooler : ObjectPooler
{
    public override void ReturnToPool(GameObject obj)
    {
        StartCoroutine(WaitForReturnObjectToPool(obj));
    }

    IEnumerator WaitForReturnObjectToPool(GameObject effect)
    {
        yield return new WaitForSeconds(effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.01f);
        effect.SetActive(false);
    }
}
