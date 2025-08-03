using UnityEngine;

public class MaceTrap : MonoBehaviour
{
    [SerializeField] private float swingSpeed;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.speed = swingSpeed;
    }
}
