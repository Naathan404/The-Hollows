using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Slab : MonoBehaviour
{
    [SerializeField] private float temp;
    private PlayerController player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (player.GetRb2D().linearVelocity.y <= 0 /*player.GetBottomPosTransform().position.y > transform.position.y + temp*/)
        {
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
