using System.Collections;
using UnityEngine;

public class Vase : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        {
            Debug.Log("1");
            StartCoroutine(WaitToBeDisappeared());
        }
    }

    IEnumerator WaitToBeDisappeared()
    {
        Debug.Log("2");
        animator.SetTrigger("Break");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
