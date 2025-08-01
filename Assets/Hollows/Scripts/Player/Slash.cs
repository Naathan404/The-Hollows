using UnityEngine;

public class Slash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<IHitable>().TakeDamage(1);
        }
    }
}
