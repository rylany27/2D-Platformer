using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
    }
}
