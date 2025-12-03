using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Collect()
    {
        animator.SetTrigger("Collected");
        StartCoroutine(DisappearItem());
    }

    IEnumerator DisappearItem()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
