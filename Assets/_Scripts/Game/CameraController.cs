using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetLock;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float shakeDuration = 5.0f;
    [SerializeField] private float shakeIntensity = 0.1f;

    public bool shake;

    private Vector3 originalPosition;
   

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        Vector3 targetPosition = targetLock.position;
        targetPosition.z = transform.position.z;
        if (shake == true)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = targetPosition + shakeOffset;

           
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    public void StartCameraShake()
    {
        originalPosition = transform.position;
        shake = true;
    }


    public void EndCameraShake()
    {
        originalPosition = transform.position;
        shake = false;

    }
}