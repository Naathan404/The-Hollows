using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Slab : MonoBehaviour
{
    [SerializeField] private float temp;
    private PlayerMovement player;
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        gameObject.tag = "Untagged";
    }

    private void Update()
    {
        if (player.GetBottomPosTransform().position.y > transform.position.y + temp)
        {
            boxCollider2D.isTrigger = false;
            gameObject.tag = "Ground";
        }
        else
        {
            boxCollider2D.isTrigger = true;
            gameObject.tag = "Untagged";
        }
    }
}
