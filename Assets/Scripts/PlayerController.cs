using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    public float moveSpeed = 1.5f;
    public float sprintSpeed = 1f;
    public float delay = 0.1f;
    public float maxHp = 20f;
    public Image hpBar;
    private float currentHp;
    
    private Rigidbody2D rb;
    private MainInputAction controls;
    [HideInInspector] public Vector2 moveInput;
    private Animator animator;
    private List<string> inputStack = new List<string>();
    private string currentDirection = "";
    private float delayTimer = 0f;
    private bool isSprinting = false;
    private bool isDead = false;
    private GameManager gameManager;

    private AudioManager audioManager;
    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        controls = new MainInputAction();
        ResetAllDirectionBools();
        animator.SetBool("isDown", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
    private void Start()
    {
        currentHp = maxHp;
        updateHpBar();
    }
    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        ProcessMovementAndAnimation();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        HandleStackUpdate("Up", moveInput.y > 0.1f);
        HandleStackUpdate("Down", moveInput.y < -0.1f);
        HandleStackUpdate("Right", moveInput.x > 0.1f);
        HandleStackUpdate("Left", moveInput.x < -0.1f);
    }

    private void HandleStackUpdate(string direction, bool isPressing)
    {
        if (isPressing)
        {
            if (!inputStack.Contains(direction)) inputStack.Add(direction);
        }
        else
        {
            inputStack.Remove(direction);
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;

        controls.Player.Sprint.performed -= OnSprint;
        controls.Player.Sprint.canceled -= OnSprint;
        controls.Player.Disable();
    }

    private void ProcessMovementAndAnimation()
    {
        string desiredDirection = inputStack.Count > 0 ? inputStack[0] : "";

        if (desiredDirection != currentDirection)
        {
            currentDirection = desiredDirection;
            if (!string.IsNullOrEmpty(currentDirection))
            {
                delayTimer = delay;
            }
        }

        if (delayTimer > 0)
        {
            delayTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero;
            UpdateAnimation(currentDirection, isDelaying: true);
            return;
        }

        Move(currentDirection);
        UpdateAnimation(currentDirection, isDelaying: false);
    }

    private void Move(string activeDirection)
    {
        Vector2 movement = Vector2.zero;
        switch (activeDirection)
        {
            case "Up": movement = Vector2.up; break;
            case "Down": movement = Vector2.down; break;
            case "Left": movement = Vector2.left; break;
            case "Right": movement = Vector2.right; break;
        }
        float currentSpeed = isSprinting ? (moveSpeed + sprintSpeed) : moveSpeed;
        rb.linearVelocity = movement * currentSpeed;
    }

    private void UpdateAnimation(string activeDirection, bool isDelaying)
    {
        bool isMoving = !string.IsNullOrEmpty(activeDirection) && !isDelaying;

        animator.SetBool("isWalking", isMoving && !isSprinting);

        animator.SetBool("isRunning", isMoving && isSprinting);

        if (!string.IsNullOrEmpty(activeDirection))
        {
            ResetAllDirectionBools();

            switch (activeDirection)
            {
                case "Up": animator.SetBool("isUp", true); break;
                case "Down": animator.SetBool("isDown", true); break;
                case "Left": animator.SetBool("isLeft", true); break;
                case "Right": animator.SetBool("isRight", true); break;
            }
        }
    }

    private void ResetAllDirectionBools()
    {
        animator.SetBool("isUp", false);
        animator.SetBool("isDown", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isRight", false);
    }

    public void takeDamage(float damage)
    {
        if (currentHp <= 0) return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        updateHpBar();
        audioManager.PlayTakeDamageSound();
        StartCoroutine(HurtEffectRoutine());

        if (currentHp <= 0) die();
    }

    IEnumerator HurtEffectRoutine()
    {
        animator.SetBool("isHurt", true);

        yield return new WaitForSeconds(0.3f);

        animator.SetBool("isHurt", false);
    }

    private void updateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void die()
    {
        if (isDead) return; 
        isDead = true;      

        animator.SetTrigger("isDeath");

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        this.enabled = false;

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    private void heal(float healItem)
    {
        currentHp += healItem;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); 
        updateHpBar();
        audioManager.PlayEatSound();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemHeal"))
        {
            heal(5);
        }
        else if (collision.CompareTag("Trap"))
        {
            takeDamage(5);
        }
        else if (collision.CompareTag("WayOut"))
        {
            gameManager.GameWin();
        }
        else if (collision.CompareTag("DeadZone"))
        {
            gameManager.GameOver();
        }
    }
}