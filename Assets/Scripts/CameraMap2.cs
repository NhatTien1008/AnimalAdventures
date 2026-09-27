using UnityEngine;

public class CameraMap2 : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public float yOffset = 0f;

    [Header("Smoothness")]
    public float smoothSpeed = 5f;
    private float defaultZ;
    private float minY;

    void Start()
    {
        defaultZ = transform.position.z;
        minY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = transform.position;
        float px = player.position.x;
        float py = player.position.y;

        if (px >= 0f && px < 124.5f && py < 3.4f && py > -2.45f)
        {
            targetPosition = new Vector3(px, 0.35f + yOffset, defaultZ);
        }
        else if (px >= 125.45f && px <= 140f && py > 0f)
        {
            targetPosition = new Vector3(132.45f, py + yOffset, defaultZ);
        }
        else if (px >= 101.75f && px < 117f && py >= 33.5f)
        {
            targetPosition = new Vector3(109.5f, py + yOffset, defaultZ);
        }
        else if (px >= 25.5f && px < 40.5f && py >= 3.4f)
        {
            targetPosition = new Vector3(33.5f, py + yOffset, defaultZ);
        }

        else if (px >= 40.5f && px < 124.5f && py >= 3.4f)
        {
            targetPosition = new Vector3(px, 30.5f + yOffset, defaultZ);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
