using System;
using UnityEngine;

public class NPCMover3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float stoppingDistance = 0.1f;
    public bool facingRightInitially;
    
    [SerializeField] private Transform targetPosition;
    private Transform targetTransform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isMoving = true;
    private Vector3 direction;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GoTo(Transform target)
    {
        targetTransform = target;
        isMoving = true;
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
        if (targetTransform == null)
        {
            direction = (targetPosition.position - transform.position).normalized;
        }
        else
        {
            direction = (targetTransform.position - transform.position).normalized;
        }

        transform.Translate(direction * speed * Time.deltaTime);
    }

    void CheckStopCondition()
    {
        if (Mathf.Abs(transform.position.x - targetPosition.position.x) < stoppingDistance)
        {
            
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

    private void OnDisable()
    {
        StopMoving();
    }
}