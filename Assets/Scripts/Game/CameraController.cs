using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetLock;

    void Update()
    {
        var pos = targetLock.position;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
