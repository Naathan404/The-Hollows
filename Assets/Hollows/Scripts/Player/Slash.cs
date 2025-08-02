using UnityEngine;

public class Slash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<IHitable>() != null)
                collision.gameObject.GetComponent<IHitable>().TakeDamage(1);
        }
    }
}
