using UnityEngine;

public class StoneGate : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpened = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        if (isOpened) return;
        animator.SetTrigger("Open");
        isOpened = true;
    }

    public void Deactivate()
    {
        if (!isOpened) return;
        animator.SetTrigger("Close");
        isOpened = false;
    }
}
