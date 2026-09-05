using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoarSkill : MonoBehaviour
{
    [Header("Charge Settings")]
    public float chargeSpeed = 6f;
    public float chargeDuration = 0.6f;
    public float cooldownTime = 4f;
    public float damage = 10f;

    public bool isCharging = false;
    public bool isCooldown = false;

    [Header("UI Update")]
    public Image cooldownImage;
    public GameObject cooldownBar;

    private PlayerController player;
    private Rigidbody2D rb;
    private MainInputAction controls;
    private Vector2 chargeDirection;
    private Coroutine chargeCoroutine;

    void Awake()
    {
        if (controls == null) controls = new MainInputAction();

        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        cooldownBar.SetActive(false);
    }

    private void OnEnable()
    {
        if (controls == null) controls = new MainInputAction();

        controls.Player.Enable();
        controls.Player.Skill.performed += OnSkillPerformed;
        controls.Player.Skill.canceled += OnSkillCanceled;
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Skill.performed -= OnSkillPerformed;
            controls.Player.Skill.canceled -= OnSkillCanceled;
            controls.Player.Disable();
        }
    }

    private void OnSkillPerformed(InputAction.CallbackContext context)
    {
        TryCharge();
    }

    private void OnSkillCanceled(InputAction.CallbackContext context)
    {

    }

    public void TryCharge()
    {
        if (!isCharging && !isCooldown)
        {
            Vector2 inputDir = (player != null) ? player.moveInput : Vector2.zero;

            if (inputDir.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y))
                {
                    chargeDirection = new Vector2(Mathf.Sign(inputDir.x), 0f);
                }
                else
                {
                    chargeDirection = new Vector2(0f, Mathf.Sign(inputDir.y));
                }
            }
            else
            {
                chargeDirection = Vector2.down;
            }

            if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
            chargeCoroutine = StartCoroutine(ChargeRoutine());
        }
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;

        if (player != null) player.enabled = false;

        float timer = 0f;

        while (timer < chargeDuration)
        {
            timer += Time.deltaTime;

            if (rb != null)
            {
                rb.linearVelocity = chargeDirection * chargeSpeed;
            }

            yield return null;
        }

        EndCharge();
    }

    private void EndCharge()
    {
        isCharging = false;

        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (player != null) player.enabled = true;

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        float timer = cooldownTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = timer / cooldownTime;
                cooldownBar.SetActive(true);
            }

            yield return null;
        }

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
            cooldownBar.SetActive(false);
        }

        isCooldown = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCharging) return;

        if (collision.gameObject.CompareTag("Breakable"))
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCharging) return;

        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<EnemyAI>(out EnemyAI enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
