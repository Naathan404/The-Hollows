using System.Collections;
using System.Linq.Expressions;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;
    private Rigidbody2D rb;
    private float velX, velY, vel0;
    private float len;
    private float angle;

    enum BombState
    {
        Airborne,
        OnGround,
        Explode
    }

    BombState state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // private void OnEnable()
    // {
    //     target = FindAnyObjectByType<PlayerController>().transform;
    //     len = Mathf.Abs(target.position.x - originPos.position.x);
    //     //angle = Mathf.Lerp(maxAngle, minAngle, len / 5f);
    //     angle = minAngle;
    //     vel0 = Mathf.Sqrt(Mathf.Abs(Physics2D.gravity.y * rb.gravityScale) * len) / Mathf.Sin(2 * angle * Mathf.Deg2Rad);
    //     velX = vel0 * Mathf.Cos(angle * Mathf.Deg2Rad) * pushForce;
    //     velY = vel0 * Mathf.Sin(angle * Mathf.Deg2Rad) * pushForce;

    //     rb.linearVelocity = new Vector2(velX, velY);
    //     StartCoroutine(Deactivate());
    // }

    public void Throw(Transform startPoint, Transform target, float dir)
    {
        this.transform.position = startPoint.position;
        len = Mathf.Abs(target.position.x - startPoint.position.x);
        angle = Mathf.Lerp(maxAngle, minAngle, len / 5f);
        vel0 = Mathf.Sqrt(Mathf.Abs(Physics2D.gravity.y * rb.gravityScale) * len) / Mathf.Sin(2 * angle * Mathf.Deg2Rad);
        velX = vel0 * Mathf.Cos(angle * Mathf.Deg2Rad) * dir; // dir == 1 if throw to the right and == -1 if throw to the left
        velY = vel0 * Mathf.Sin(angle * Mathf.Deg2Rad);

        rb.linearVelocity = new Vector2(velX, velY);
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}