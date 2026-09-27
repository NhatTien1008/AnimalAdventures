using UnityEngine;

public class CameraMap3 : MonoBehaviour
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

        if (px >= 0f && px <= 91f && py >= -2.5f)
        {
            targetPosition = new Vector3(px, 0f + yOffset, defaultZ);
        }
        else if (px > 91f && px <= 107f && py < 45f && py >= -2.5f)
        {
            targetPosition = new Vector3(99.45f, py + yOffset, defaultZ);
        }
        else if (px > 107f && px <=193.5f && py < 47.5f && py >= -2.5f)
        {
            targetPosition = new Vector3(px, 44.5f + yOffset, defaultZ);
        }
        else if (px > 193.5f && px <= 209f && py < 45f && py >= 0f)
        {
            targetPosition = new Vector3(201.5f, py + yOffset, defaultZ);
        }
        else if (px >= 209.5f && px <= 251.5f && py < 4.5f && py >= -2.5f)
        {
            targetPosition = new Vector3(px, 0f + yOffset, defaultZ);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
