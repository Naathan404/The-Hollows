using System.Collections;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationTime;
    private bool isFacingRight;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        isFacingRight = playerMovement.IsFacingRight();
    }

    private void Update()
    {
        transform.position = player.position;
    }

    public void TurnAround()
    {
        StartCoroutine(RotateYAxis());
    }

    IEnumerator RotateYAxis()
    {
        float start = transform.localEulerAngles.y;
        float end = DetermineEndRotation();
        isFacingRight = !isFacingRight;
        float yRotation = 0f;
        float timer = 0f;
        while (timer < rotationTime)
        {
            timer += Time.deltaTime;

            yRotation = Mathf.Lerp(start, end, timer / rotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }   
    }

    private float DetermineEndRotation()
    {
        if (isFacingRight)
        {
            return 180f;
        }
        return 0f;
    }
}
