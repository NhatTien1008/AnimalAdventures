using UnityEngine;
using UnityEngine.InputSystem;

public class FoxSkill : MonoBehaviour
{
    private MainInputAction controls;
    private SpriteRenderer spriteRenderer;

    private bool isInBush = false;
    private bool isHoldingKey = false;

    private int originalLayer;
    private int hiddenLayer;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new MainInputAction();
        originalLayer = gameObject.layer;
        hiddenLayer = LayerMask.NameToLayer("Hidden");
    }

    private void OnSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingKey = true;
            TryEnterStealth();
        }
        else if (context.canceled)
        {
            isHoldingKey = false;
            ExitStealth();
        }
    }

    private void TryEnterStealth()
    {
        if (isHoldingKey && isInBush)
        {
            gameObject.layer = hiddenLayer;
            SetAlpha(0.5f);
            audioManager.FoxSkillHide();
        }
    }

    private void ExitStealth()
    {
        gameObject.layer = originalLayer;
        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bush"))
        {
            isInBush = true;

            if (isHoldingKey) TryEnterStealth();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bush"))
        {
            isInBush = false;
            ExitStealth();
        }
    }

    private void OnEnable()
    {
        controls.Player.Skill.performed += OnSkill;
        controls.Player.Skill.canceled += OnSkill;
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Skill.performed -= OnSkill;
        controls.Player.Skill.canceled -= OnSkill;
        controls.Player.Disable();
    }
}
