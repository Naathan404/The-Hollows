using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] EffectPooler breakDusts;
    [SerializeField] private float rayDis;
    private RaycastHit2D hitGround;
    private void Start()
    {
    }

    private void Update()
    {
        hitGround = Physics2D.Raycast(transform.position, -Vector2.up, rayDis, LayerMask.GetMask("Ground"));
        if (hitGround)
        {
            GameObject dust = breakDusts.GetObject();
            dust.transform.position = transform.position - new Vector3(0f, rayDis);
            //Debug.Log("Hit object");
            gameObject.SetActive(false);
            breakDusts.ReturnToPool(dust);
        }

        Debug.DrawRay(transform.position, -Vector2.up * rayDis, Color.red);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject dust = breakDusts.GetObject();
            dust.transform.position = transform.position;
            gameObject.SetActive(false);
            breakDusts.ReturnToPool(dust);
        }
    }
}
