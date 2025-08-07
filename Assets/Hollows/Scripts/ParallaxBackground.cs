using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject followObj;
    [SerializeField] private float speed;
    private SpriteRenderer spriteRenderer;
    private float backgroundWidth;
    private float distance;
    private float startPoint;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundWidth = spriteRenderer.sprite.bounds.size.x;
        startPoint = transform.position.x;
        followObj = GameObject.Find("CamFollowObj");
    }

    private void Update()
    {
        float movement = followObj.transform.position.x * (1f - speed);
        if (movement > startPoint + backgroundWidth)
        {
            startPoint += backgroundWidth;
        }
        if (movement < startPoint - backgroundWidth)
        {
            startPoint -= backgroundWidth;
        }
    }

    private void FixedUpdate()
    {
        distance = followObj.transform.position.x * speed;
        transform.position = new Vector3(startPoint - distance, transform.position.y, transform.position.z);
    }
}
