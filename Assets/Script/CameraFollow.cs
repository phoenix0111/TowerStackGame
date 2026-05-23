using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform crane;

    public float yOffset = 5f;

    public float smoothTime = 0.3f;

    Vector3 velocity;

    void Update()
    {
        Vector3 targetPos = transform.position;

        targetPos.y = crane.position.y + yOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime );
    }
}