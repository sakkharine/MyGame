using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SwampMovement1 : MonoBehaviour
{
    [Header("Скорости и силы")]
    public float walkSpeed = 2f;          // безопасная скорость движения
    public float fastFallSpeed = 3f;      // скорость проваливания при быстром движении
    public float dragDownSpeed = 1f;      // скорость затягивания при стоянии
    public float sinkLerpSpeed = 2f;      // плавность погружения

    [Header("Проверка болота")]
    public LayerMask swampLayer;

    [Header("Эффект погружения")]
    public float maxSink = 0.3f;  // максимально насколько персонаж "погружается" в болото

    [Header("Отладка")]
    public bool verbose = true;

    private Rigidbody2D rb;
    private bool inSwamp = false;
    private float inputX;
    private float targetYOffset = 0f;
    private float currentYOffset = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        // Проверка нахождения в болоте через Raycast вниз
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, swampLayer);
        inSwamp = hit.collider != null;

        // Дебаг луча
        if (verbose)
        {
            Debug.DrawRay(transform.position, Vector2.down, inSwamp ? Color.green : Color.red);
            Debug.Log($"InSwamp: {inSwamp}, inputX: {inputX:F2}");
        }

        // Определяем целевой уровень погружения
        if (!inSwamp)
        {
            targetYOffset = 0f;
        }
        else
        {
            if (Mathf.Abs(inputX) > 1f)
            {
                targetYOffset = maxSink; // слишком быстро → провал
                if (verbose) Debug.Log("Fast movement: sinking");
            }
            else if (Mathf.Abs(inputX) < 0.1f)
            {
                targetYOffset = maxSink; // стоим → затягивает
                if (verbose) Debug.Log("Standing still: sinking");
            }
            else
            {
                targetYOffset = maxSink / 2f; // медленно идём → частичное погружение
                if (verbose) Debug.Log("Walking slowly: slight sink");
            }
        }

        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * sinkLerpSpeed);
    }

    void FixedUpdate()
    {
        if (!inSwamp)
        {
            rb.velocity = new Vector2(inputX * walkSpeed * 3f, rb.velocity.y);
            return;
        }

        // Движение по болоту
        float speed = Mathf.Clamp(inputX * walkSpeed, -walkSpeed, walkSpeed);
        rb.velocity = new Vector2(speed, -dragDownSpeed);

        // Визуальное погружение (Y-позиция)
        Vector3 pos = transform.position;
        pos.y -= currentYOffset;
        transform.position = pos;
    }
}
