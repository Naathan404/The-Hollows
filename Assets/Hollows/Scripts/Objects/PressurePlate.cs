using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private float rayLen;
    [SerializeField] private LayerMask objectLayerMask;
    [SerializeField] private GameObject interactiveObject;
    private RaycastHit2D hitObject;
    private Animator animator;
    private IInteractable interactable;

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactable = interactiveObject?.GetComponent<IInteractable>();
    }

    private void Update()
    {
        hitObject = Physics2D.Raycast(this.transform.position, Vector2.up, rayLen, objectLayerMask);

        if (hitObject)
        {
            Debug.Log($"Hit {hitObject.collider.gameObject.name}");
            animator.SetBool("IsActive", true);
            interactable.Activate();
        }
        else
        {
            animator.SetBool("IsActive", false);
            interactable.Deactivate();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * rayLen);
    }
}
