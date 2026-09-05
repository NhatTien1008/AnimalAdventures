using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum PatrolAxis { Horizontal, Vertical }

    [Header("Patrol Settings")]
    public float moveSpeed = 1.5f;
    public float patrolDistance = 3f;
    public PatrolAxis patrolDirection = PatrolAxis.Horizontal;

    [Header("Combat Settings")]
    public Transform player;
    public float attackRange = 1.25f;
    public float attackCooldown = 3f;
    public float enemyHeal = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingToTarget = true;

    private float nextAttackTime = 0f;
    private bool isAttacking = false;
    private bool isWaiting = false;
    public float waitTime = 1.5f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;

        if (patrolDirection == PatrolAxis.Horizontal)
        {
            targetPosition = startPosition + new Vector2(patrolDistance, 0);
        }
        else
        {
            targetPosition = startPosition + new Vector2(0, patrolDistance);
        }
    }
    void FixedUpdate()
    {
        if (player == null || isAttacking) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && IsPlayerInSight())
        {
            Attack();
        }
        else
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        if (isWaiting) return;

        Vector2 currentGoal = movingToTarget ? targetPosition : startPosition;
        Vector2 direction = (currentGoal - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
        animator.SetBool("isWalking", true);

        UpdateDirectionAnimation(direction);

        if (Vector2.Distance(transform.position, currentGoal) < 0.1f)
        {
            StartCoroutine(WaitBeforeTurning());
            animator.SetBool("isWalking", false);
        }
    }
    IEnumerator WaitBeforeTurning()
    {
        isWaiting = true;
        movingToTarget = !movingToTarget;
        Vector2 nextGoal = movingToTarget ? targetPosition : startPosition;
        Vector2 nextDirection = (nextGoal - (Vector2)transform.position).normalized;
        UpdateDirectionAnimation(nextDirection);
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
    }
    private void UpdateDirectionAnimation(Vector2 dir)
    {
        if (patrolDirection == PatrolAxis.Horizontal)
        {
            animator.SetBool("isRight", dir.x > 0);
            animator.SetBool("isLeft", dir.x < 0);
            animator.SetBool("isUp", false);
            animator.SetBool("isDown", false);
        }
        else
        {
            animator.SetBool("isUp", dir.y > 0);
            animator.SetBool("isDown", dir.y < 0);
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
        }
    }
    private void Attack()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWalking", false);
        isAttacking = true;
        animator.SetBool("isAttack", true);
        Vector2 dir = (player.position - transform.position);
        if (patrolDirection == PatrolAxis.Horizontal)
        {
            UpdateDirectionAnimation(new Vector2(dir.x > 0 ? 1 : -1, 0));
        }
        else
        {
            UpdateDirectionAnimation(new Vector2(0, dir.y > 0 ? 1 : -1));
        }
        nextAttackTime = Time.time + attackCooldown;
        Invoke(nameof(ResetAttackState), 0.7f);
    }
    private bool IsPlayerInSight()
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Hidden"))
        {
            return false;
        }

        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (patrolDirection == PatrolAxis.Horizontal)
        {
            if (animator.GetBool("isRight"))
            {
                return directionToPlayer.x > 0.2f;
            }
            if (animator.GetBool("isLeft"))
            {
                return directionToPlayer.x < -0.2f;
            }
        }
        else
        {
            if (animator.GetBool("isUp"))
            {
                return directionToPlayer.y > 0.2f;
            }
            if (animator.GetBool("isDown"))
            {
                return directionToPlayer.y < -0.2f;
            }
        }

        return false;
    }
    private void ResetAttackState()
    {
        isAttacking = false;
        animator.SetBool("isAttack", false);
    }
    public void HitPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && IsPlayerInSight())
        {
            player.GetComponent<PlayerController>().takeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHeal -= damage;

        if (animator != null)
        {
            animator.SetTrigger("isHurt");
        }
        if (enemyHeal <= 0)
        {
            Destroy(gameObject);
        }
    }
}