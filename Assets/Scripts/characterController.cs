using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;
    public float groundDrag = 5f;
    public float airDrag = 0.5f;

    [Header("Jump/Fly")]
    public float jumpVelocity = 12f;
    public float flyingForce = 80f;
    public float flyingTime = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask whatIsGround;

    [Header("Components")]
    [SerializeField] private Animator anim;
    private Rigidbody2D rb2d;
    private Collider2D col;

    private bool facingRight = true;
    private bool grounded = false;
    private bool hasFlyAbility = false;
    private bool canFly = false;
    private bool isJumping = false;
    private Coroutine flightCoroutine;

    private const string STATE_PARAM = "state";
    private const string JUMP_TRIGGER = "jumpTrigger";
    private const string X_SPEED_PARAM = "xSpeed";
    private const string Y_SPEED_PARAM = "ySpeed";
    private const string GROUNDED_PARAM = "isGrounded";

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
        UpdateAnimatorParameters();
    }

    float inputHorizontal = 0f;
    bool inputJumpDown = false;

    void HandleInput()
    {
        inputHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            inputJumpDown = true;

        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (!wasGrounded && grounded)
        {
            isJumping = false;
            if (hasFlyAbility) canFly = true;
        }
        MoveHorizontal(inputHorizontal); 

        if (inputJumpDown)
        { 
            ProcessJumpOrFly(); 
            inputJumpDown = false; 
        }
        rb2d.drag = grounded ? groundDrag : airDrag;
    }

    void MoveHorizontal(float move)
    {
        float targetX = move * maxSpeed;
        float velY = rb2d.velocity.y;
        float smoothing = grounded ? 0.9f : 0.98f;
        float newX = Mathf.Lerp(rb2d.velocity.x, targetX, 1f - smoothing);

        if (grounded && Mathf.Abs(move) < 0.01f)
            newX = 0f;
        if (grounded && Mathf.Abs(move) > 0.1f)
        {
            if (!IsInvoking("PlayFootstep"))
                Invoke("PlayFootstep", 0.8f);
        }


        rb2d.velocity = new Vector2(newX, velY);

        if (move > 0.01f && !facingRight) Flip();
        else if (move < -0.01f && facingRight) Flip();
    }

    void ProcessJumpOrFly()
    {
        if (grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
            isJumping = true;
            grounded = false;
            TriggerJumpAnimation();
            return;
        }

        if (hasFlyAbility && canFly)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(Vector2.up * flyingForce, ForceMode2D.Impulse);
            isJumping = true;
            TriggerJumpAnimation();

            if (flightCoroutine != null)
                StopCoroutine(flightCoroutine);
            flightCoroutine = StartCoroutine(FlightDisableTimer(flyingTime));

            return;
        }
    }

    IEnumerator FlightDisableTimer(float seconds)
    {
        canFly = false; // чтобы нельзя было спамить
        yield return new WaitForSeconds(seconds);
        canFly = false;
        flightCoroutine = null;
    }

    private States State
    {
        get { return (States)anim.GetInteger(STATE_PARAM); }
        set
        {
            if ((int)value != anim.GetInteger(STATE_PARAM))
                anim.SetInteger(STATE_PARAM, (int)value);
        }
    }

    void PlayFootstep()
    {
        SoundManager.PlayRandom("Footstep");
    }

    void TriggerJumpAnimation()
    {
        if (anim == null) return;
        anim.SetTrigger(JUMP_TRIGGER);
        State = States.girl_jump;
    }

    void UpdateAnimatorParameters()
    {
        if (anim == null) return;

        anim.SetBool(GROUNDED_PARAM, grounded);
        anim.SetFloat(X_SPEED_PARAM, Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat(Y_SPEED_PARAM, rb2d.velocity.y);

        float hspeed = Mathf.Abs(rb2d.velocity.x);
        if (isJumping || !grounded)
            State = States.girl_jump;
        else if (grounded)
            State = hspeed > 0.1f ? States.girl_run : States.girl_idle;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    public void GiveFlyAbility()
    {
        hasFlyAbility = true;
        canFly = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "dieCollider" || col.gameObject.name == "foxi")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum States
{
    girl_idle,
    girl_run,
    girl_jump,
    speed,
    wounded,
    dead
}
