using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetLock;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float shakeDuration = 5.0f;
    [SerializeField] private float shakeIntensity = 0.1f;

    private Vector3 originalPosition;
    private float shakeTimer = 0f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position += shakeOffset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            Vector3 targetPosition = targetLock.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    public void StartCameraShake()
    {
        originalPosition = transform.position;
        shakeTimer = shakeDuration;
    }
}