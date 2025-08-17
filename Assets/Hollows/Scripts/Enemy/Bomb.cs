using System.Collections;
using System.Linq.Expressions;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    private EffectPooler explosionEffectPool;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerController playerController;
    private float velX, velY, vel0;
    private float len;
    private float angle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        explosionEffectPool = GameObject.Find("ExplosionPool").GetComponent<EffectPooler>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    public void Throw(Transform startPoint, Transform target, float dir)
    {
        this.transform.position = startPoint.position;
        len = Mathf.Abs(target.position.x - startPoint.position.x);
        angle = Mathf.Lerp(maxAngle, minAngle, len / 5f);
        vel0 = Mathf.Sqrt(Mathf.Abs(Physics2D.gravity.y * rb.gravityScale) * len) / Mathf.Sin(2 * angle * Mathf.Deg2Rad);
        velX = vel0 * Mathf.Cos(angle * Mathf.Deg2Rad) * dir;
        velY = vel0 * Mathf.Sin(angle * Mathf.Deg2Rad);

        rb.linearVelocity = new Vector2(velX, velY);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetTrigger("HitGround");
            StartCoroutine(WaitForExploding());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject effect = explosionEffectPool.GetObject();
            effect.transform.position = this.transform.position;
            explosionEffectPool.ReturnToPool(effect);
            gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForExploding()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject effect = explosionEffectPool.GetObject();
        effect.transform.position = this.transform.position;
        explosionEffectPool.ReturnToPool(effect);
        gameObject.SetActive(false);
    }
}