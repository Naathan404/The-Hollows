using UnityEngine;

public class Sign : MonoBehaviour
{
   [SerializeField] private GameObject content;

    private void Start()
    {
        if(content.activeInHierarchy)
            content.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            content.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            content.SetActive(false);
        }
    }
}
