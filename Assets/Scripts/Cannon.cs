using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform firePoint;     
    public Vector2 fireDirection = Vector2.left;
    public float fireRate = 2.0f; 

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            if (animator != null)
                animator.SetBool("Fire", true);

            yield return new WaitForSeconds(0.15f);

            yield return new WaitForSeconds(0.2f);
            if (animator != null)
                animator.SetBool("Fire", false);
        }
    }

    public void Shoot()
    {
        if (ballPrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);

            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.SetDirection(fireDirection);
            }
        }
    }
}
