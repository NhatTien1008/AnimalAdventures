using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public float yOffset = 0f;

    [Header("Smoothness")]
    public float smoothSpeed = 5f;

    private float defaultX;
    private float defaultZ;
    private float minY;

    void Start()
    {
        defaultX = transform.position.x;
        defaultZ = transform.position.z;
        minY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player == null || player.position.y <= minY) return;

        Vector3 targetPosition = transform.position;
        float px = player.position.x;
        float py = player.position.y;

 
        if (py >= 64.3f && Mathf.Abs(px) <= 29f)
        {
            targetPosition = new Vector3(px, 66.5f + yOffset, defaultZ);
        }

        else if (px >= -15.2f && px < -3.5 && py > 37.5f)
        {
            targetPosition = new Vector3(-9.5f, py + yOffset, defaultZ);
        }

        else if (px >= -15.2 && px < 3.5 && py >= 32.27f && py <= 37.5f)
        {
            targetPosition = new Vector3(px, 35f + yOffset, defaultZ);
        }

        else if (px >= 3.5 && px < 15.2 && py >= 32.27f && py <56f)
        {
            targetPosition = new Vector3(9.5f, py + yOffset, defaultZ);
        }

        else if (py < 20f)
        {
            targetPosition = new Vector3(defaultX, py + yOffset, defaultZ);
        }

        else if (Mathf.Abs(px) < 23f)
        {
            targetPosition = new Vector3(px, 23f + yOffset, defaultZ);
        }

        else if (Mathf.Abs(px) >= 23f && Mathf.Abs(px) <= 35f)
        {
            float lockedX = (px < 0) ? -29f : 29f;
            targetPosition = new Vector3(lockedX, py + yOffset, defaultZ);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}