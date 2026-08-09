using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 5f;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        transform.Translate(moveDirection * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.takeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("MapLimit"))
        {
            Destroy(gameObject);
        }
    }
}
