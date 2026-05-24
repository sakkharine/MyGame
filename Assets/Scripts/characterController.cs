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
    public float stopCheckAfterJumpTime = 0.1f;

    [Header("Components")]
    [SerializeField] private Animator anim;
    private Rigidbody2D rb2d;
    private Collider2D col;

    [Header("Limits")] 
    [SerializeField] public bool canMoveX = true;
    [SerializeField] private bool hasFlyAbility = false;
    
    [Space]
    public bool facingRight = true;
    
    public Vector3 FaceDirection => facingRight ? Vector3.right : Vector3.left;
    
    public Vector2 SecondaryVelocity { get; set; }
    
    private bool grounded = false;
    private bool canFly = false;
    private bool isJumping = false;
    private float jumpTime;
    
    private Coroutine flightCoroutine;

    private const string STATE_PARAM = "state";
    private const string JUMP_TRIGGER = "jumpTrigger";
    private const string X_SPEED_PARAM = "xSpeed";
    private const string Y_SPEED_PARAM = "ySpeed";
    private const string GROUNDED_PARAM = "isGrounded";

    private ContactFilter2D _contactFilter;
    
    void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb2d.freezeRotation = true;
        rb2d.drag = groundDrag;

        _contactFilter.NoFilter();
        _contactFilter.useTriggers = false;
        _contactFilter.useLayerMask = true;
        _contactFilter.layerMask = whatIsGround;
    }

    private void OnEnable()
    {
        facingRight = transform.localScale.x > 0f; 
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
        
        inputHorizontal += MobileInput.Instance.Horizontal;
        
        if (MobileInput.Instance.JumpDown) inputJumpDown = true;
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius * Mathf.Abs(transform.lossyScale.x), _contactFilter, _colliderBuffer) > 0;
        
        if(Time.time < jumpTime + stopCheckAfterJumpTime)
            grounded = false;
        
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

        if (canMoveX)
        {
            int size = Physics2D.CircleCastNonAlloc(groundCheck.position, groundRadius, Vector2.down, raycastHitBuffer, 1f, whatIsGround);

            RaycastHit2D? hit = null;
            for (int i = 0; i < size; i++)
            {
                hit = raycastHitBuffer[i];
                if (hit.Value.collider == this.col || hit.Value.collider.isTrigger)
                {
                    hit = null;
                }
                else
                {
                    break;
                }
            }
            
            Vector2 newVelocity = Vector3.Cross(hit.HasValue ? hit.Value.normal : Vector2.up, Vector3.forward) * newX;
            Vector2 totalMovement = newVelocity;

            if (!grounded)
                totalMovement.y = rb2d.velocity.y;
    
            rb2d.velocity = totalMovement;
        }

        var influencedMovement = new Vector2(
            CalculateInfluence(rb2d.velocity.x, SecondaryVelocity.x),
            CalculateInfluence(rb2d.velocity.y, SecondaryVelocity.y)
        );

        rb2d.velocity = influencedMovement;
        
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
            jumpTime = Time.time;
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
            jumpTime = Time.time;
            
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

    private States _state;
    private RaycastHit2D[] raycastHitBuffer = new RaycastHit2D[8];
    private Collider2D[] _colliderBuffer = new Collider2D[8];

    private States State
    {
        get => _state;
        set
        {
            if (value != _state)
            {
                if (anim != null)
                {
                    anim.SetInteger(STATE_PARAM, (int)value);
                }
                _state = value;
            }
        }
    }

    void PlayFootstep()
    {
        SoundManager.PlayRandom("Footstep");
    }

    void TriggerJumpAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger(JUMP_TRIGGER);
        };
        State = States.girl_jump;
    }

    void UpdateAnimatorParameters()
    {
        float hspeed = Mathf.Abs(inputHorizontal);
        
        if (anim != null)
        {
            anim.SetBool(GROUNDED_PARAM, grounded);
            anim.SetFloat(X_SPEED_PARAM, hspeed);
            anim.SetFloat(Y_SPEED_PARAM, rb2d.velocity.y);
        }

        if (isJumping || !grounded)
            State = States.girl_jump;
        else if (grounded)
            State = hspeed > 0.1f ? States.girl_run : States.girl_idle;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    public void GiveFlyAbility()
    {
        hasFlyAbility = true;
        canFly = true;
    }

    public void SetXLimit(bool canMove)
    {
        canMoveX = canMove;
    }
    
    public void Stop()
    {
        rb2d.velocity = Vector2.zero;
        UpdateAnimatorParameters();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "dieCollider" || col.gameObject.name == "foxi")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        UpdateAnimatorParameters();
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius * Mathf.Abs(transform.lossyScale.x));
        }
    }
    
    private float CalculateInfluence(float target, float influence)
    {
        if (target == 0f)
            return influence;

        if (Mathf.Sign(target) == Mathf.Sign(influence))
        {
            return Mathf.Max(target, influence);
        }
        else
        {
            return target - influence;
        }
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
