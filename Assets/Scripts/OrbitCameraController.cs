using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 20f;
    public float xSpeed = 120f;
    public float ySpeed = 90f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float zoomSpeed = 5f;
    public float minDistance = 5f;
    public float maxDistance = 60f;

    private float x;
    private float y;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (target == null)
        {
            GameObject fallback = new GameObject("CameraTarget");
            fallback.transform.position = Vector3.zero;
            target = fallback.transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0f, 0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
