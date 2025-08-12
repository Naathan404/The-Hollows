using System.Collections;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rotationTime;
    private bool isFacingRight;
    private PlayerController playerController;

    private void Awake()
    {
    }

    private void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerController>().transform;
        playerController = playerTransform.gameObject.GetComponent<PlayerController>();
        isFacingRight = playerController.IsFacingRight();
    }

    private void Update()
    {
        transform.position = playerTransform.position;
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
