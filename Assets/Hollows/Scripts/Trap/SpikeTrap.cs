using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private GameObject spike;
    [SerializeField] private float firstTriggerCoolDown;
    [SerializeField] private float triggerCoolDown;
    [SerializeField] private float counter;
    public float timeToWait;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (spike.activeInHierarchy)
            spike.SetActive(false);
        counter = firstTriggerCoolDown;
    }

    private void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0f)
        {
            animator.SetTrigger("TrapTrigger");
            StartCoroutine(WaitForAnimation(timeToWait));
            // Reset counter
            counter = triggerCoolDown;
        }
    }

    IEnumerator WaitForAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        spike.transform.position = this.transform.position;
        spike.SetActive(true);
    }
}
