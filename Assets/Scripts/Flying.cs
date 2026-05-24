using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying : MonoBehaviour
{
     [Header("Movement")]
    public float maxSpeed = 8f;
    public float groundDrag = 5f;
    public float airDrag = 0.5f;

    [Header("Jump/Fly")]
    public float jumpVelocity = 12f;
    public float flyingForce = 25f; // уменьшил для контроля
    public float maxFlySpeed = 6f;  // ограничение скорости вверх
    public float flyingTime = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask whatIsGround;

    [Header("Components")]
    [SerializeField] private Animator anim;
    private Rigidbody2D rb2d;
    private Collider2D col;

    public Vector3 FaceDirection => facingRight ? Vector3.right : Vector3.left;

    public bool facingRight = true;
    private bool grounded = false;
    [SerializeField] private bool hasFlyAbility = false;

    private bool isFlying = false;
    private float flyTimer = 0f;

    private bool isJumping = false;

    private const string STATE_PARAM = "state";
    private const string JUMP_TRIGGER = "jumpTrigger";
    private const string X_SPEED_PARAM = "xSpeed";
    private const string Y_SPEED_PARAM = "ySpeed";
    private const string GROUNDED_PARAM = "isGrounded";

    float inputHorizontal = 0f;
    bool inputJumpDown = false;
    bool inputJumpHold = false;

    void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb2d.freezeRotation = true;
        rb2d.drag = groundDrag;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        inputJumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        inputJumpHold = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();

        inputHorizontal += MobileInput.Instance.Horizontal;

        if (MobileInput.Instance.JumpDown)
        {
            inputJumpDown = true;
            inputJumpHold = true;
        }
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (grounded)
        {
            isFlying = false;
            flyTimer = 0f;
            isJumping = false;
        }

        MoveHorizontal(inputHorizontal);

        HandleFlying();
    }

    void MoveHorizontal(float move)
    {
        float targetX = move * maxSpeed;
        float velY = rb2d.velocity.y;
        float smoothing = grounded ? 0.9f : 0.98f;

        float newX = Mathf.Lerp(rb2d.velocity.x, targetX, 1f - smoothing);

        if (grounded && Mathf.Abs(move) < 0.01f)
            newX = 0f;

        rb2d.velocity = new Vector2(newX, velY);

        if (move > 0.01f && !facingRight) Flip();
        else if (move < -0.01f && facingRight) Flip();
    }

    void HandleFlying()
    {
        if (inputJumpHold)
        {
            if (rb2d.velocity.y < maxFlySpeed)
            {
                rb2d.AddForce(Vector2.up * flyingForce);
            }
        }
    }

    private States _state;
    private States State
    {
        get => _state;
        set
        {
            if (value != _state)
            {
                if (anim != null)
                    anim.SetInteger(STATE_PARAM, (int)value);

                _state = value;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
