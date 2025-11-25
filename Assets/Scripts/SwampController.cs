using UnityEngine;

public class SwampController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxSafeSpeed = 2f;       // максимальная безопасная скорость
    public float idleTimeToSink = 1.5f;   // сколько секунд можно стоять
    public float sinkSpeed = 1f;          // скорость накопления "болотности"
    public float sinkLimit = 3f;          // если погружение достигло лимита — смерть

    public float sinkDepth = 1f;          // глубина, на которую персонаж уходит под землю
    public float verticalSinkSpeed = 0.5f;// скорость вертикального опускания

    private float idleTimer = 0f;
    private float sinkAmount = 0f;

    private Vector2 lastPosition;
    private float startY;
    private bool isSinking = false;

    void Start()
    {
        lastPosition = rb.position;
        startY = rb.position.y;   // запоминаем начальную высоту
    }

    void Update()
    {
        float speed = (rb.position - lastPosition).magnitude / Time.deltaTime;

        bool isStanding = speed < 0.05f;
        bool isTooFast = speed > maxSafeSpeed;

        // Логика накопления погружения
        if (isStanding)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > idleTimeToSink)
                sinkAmount += sinkSpeed * Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;

            if (isTooFast)
                sinkAmount += sinkSpeed * Time.deltaTime;
            else
                sinkAmount = Mathf.Max(0, sinkAmount - Time.deltaTime);
        }

        // Начать вертикальное опускание, если началось погружение
        isSinking = sinkAmount > 0;

        // Вертикальное опускание
        if (isSinking)
        {
            float targetY = startY - (sinkAmount / sinkLimit) * sinkDepth;
            float newY = Mathf.Lerp(rb.position.y, targetY, verticalSinkSpeed * Time.deltaTime);
            rb.position = new Vector2(rb.position.x, newY);
        }
        else
        {
            // возвращаем персонажа обратно, если выбрался
            float newY = Mathf.Lerp(rb.position.y, startY, verticalSinkSpeed * Time.deltaTime);
            rb.position = new Vector2(rb.position.x, newY);
        }

        lastPosition = rb.position;

        // Проверка смерти
        if (sinkAmount >= sinkLimit)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Персонаж полностью ушёл под болото!");
        // Здесь можно вызвать анимацию смерти, рестарт уровня и т.п.
    }
}
