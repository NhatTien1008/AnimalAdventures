using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GrouseSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public float flyTime = 5f;
    public float cooldownTime = 10f;
    public bool isFly = false;
    public bool isCooldown = false;

    [Header("UI Update")]
    public Image cooldownImage;
    public GameObject cooldownBar;

    private MainInputAction controls;
    private SpriteRenderer spriteRenderer;
    private Coroutine flyCoroutine;
    private Coroutine cooldownCoroutine;
    public bool isHoldingSkillKey;

    private int originalLayer;
    private int flyLayer;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        controls = new MainInputAction();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalLayer = gameObject.layer;
        flyLayer = LayerMask.NameToLayer("Fly");
        cooldownBar.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Skill.performed += OnSkillPerformed;
        controls.Player.Skill.canceled += OnSkillCanceled;
    }

    private void OnDisable()
    {
        controls.Player.Skill.performed -= OnSkillPerformed;
        controls.Player.Skill.canceled -= OnSkillCanceled;
        controls.Player.Disable();
    }

    private void OnSkillPerformed(InputAction.CallbackContext context)
    {
        isHoldingSkillKey = true;
    }

    private void OnSkillCanceled(InputAction.CallbackContext context)
    {
        isHoldingSkillKey = false;
        CancelFly();
    }

    public void TryStartFly()
    {
        if (!isFly && isHoldingSkillKey && !isCooldown)
        {
            if (flyCoroutine != null) StopCoroutine(flyCoroutine);
            flyCoroutine = StartCoroutine(FlyRoutine());
        }
    }

    public void CancelFly()
    {
        if (isFly)
        {
            if (flyCoroutine != null) StopCoroutine(flyCoroutine);
            EndFlyAndStartCooldown();
        }
    }

    IEnumerator FlyRoutine()
    {
        StartFlying();

        yield return new WaitForSeconds(flyTime);

        EndFlyAndStartCooldown();
    }

    private void EndFlyAndStartCooldown()
    {
        StopFlying();

        if (!isCooldown)
        {
            if (cooldownCoroutine != null) StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = StartCoroutine(CooldownRoutine());
        }
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

    private void StartFlying()
    {
        isFly = true;
        gameObject.layer = flyLayer;
        SetAlpha(0.5f);
        audioManager.GrouseSkillFly();
    }

    private void StopFlying()
    {
        isFly = false;
        gameObject.layer = originalLayer;
        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;
        }
    }
}
