using UnityEngine;

public class NPCMover2D : MonoBehaviour
{
    public enum MoveMode
    {
        Distance,
        Time
    }

    [Header("Movement Settings")]
    public float speed = 2f;
    public MoveMode moveMode = MoveMode.Distance;

    [Header("Distance Mode")]
    public float moveDistance = 5f;

    [Header("Time Mode")]
    public float moveTime = 3f;

    [Header("Direction")]
    public Vector2 direction = Vector2.right;

    [Header("Sprite Settings")]
    public bool facingRightInitially = true;

    private Vector2 startPosition;
    private float timer;
    private bool isMoving = true;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        direction.Normalize();

        UpdateSpriteDirection();
    }

    void Update()
    {
        if (!isMoving)
            return;

        Move();
        CheckStopCondition();
        UpdateAnimation();
        UpdateSpriteDirection();
    }

    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        timer += Time.deltaTime;
    }

    void CheckStopCondition()
    {
        if (moveMode == MoveMode.Distance)
        {
            float distance = Vector2.Distance(startPosition, transform.position);
            if (distance >= moveDistance)
            {
                StopMoving();
            }
        }
        else
        {
            if (timer >= moveTime)
            {
                StopMoving();
            }
        }
    }

    void StopMoving()
    {
        isMoving = false;
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    void UpdateSpriteDirection()
    {
        if (spriteRenderer == null)
            return;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = !facingRightInitially;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = facingRightInitially;
        }
    }
}