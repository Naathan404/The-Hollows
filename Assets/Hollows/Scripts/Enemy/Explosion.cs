using UnityEngine;

public class Explosion : MonoBehaviour
{
    private PlayerController playerController;
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.TakeDamage(1);
        }
    }
}
