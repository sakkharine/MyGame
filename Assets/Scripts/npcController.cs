using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NpcController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;           // обычная скорость
    public float boostedSpeed = 6f;        // ускоренная скорость после "пробуждения"
    public float dashDuration = 0.3f;      // длительность рывка
    public float dashInterval = 1.5f;      // пауза между рывками
    public float delayBeforeBoost = 5f;    // через сколько секунд после старта начнутся рывки
    public bool moveRight = true;          // направление движения

    [Header("Reaction Settings")]
    public float reactionDelay = 0.5f;     // задержка перед началом анимации реакции

    [Header("Components")]
    private Animator anim;
    private bool isMoving = false;
    private bool isReacting = false;
    private bool isBoosted = false;
    private MusicController music;

    void Start()
    {
        anim = GetComponent<Animator>();
        music = FindObjectOfType<MusicController>();
    }

    void Update()
    {
        if (isMoving)
        {
            float direction = moveRight ? 1f : -1f;
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isReacting)
            {
                music.SetDrums(true);
                // Запускаем реакцию с задержкой (например, 0.5 сек)
                Invoke(nameof(StartReaction), reactionDelay);
            }
            else
            {
                // если игрок дотронулся во время реакции — перезапускаем сцену
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void StartReaction()
    {
        isReacting = true;

        if (anim != null)
            anim.SetTrigger("React"); // проигрываем анимацию реакции

        // После проигрывания анимации (можно задать фиксированную задержку)
        Invoke(nameof(StartMoving), 1f); // 1 сек — длительность анимации реакции
    }

    void StartMoving()
    {
        isMoving = true;
        // Через delayBeforeBoost секунд начинаем ускоряться и "рывкать"
        StartCoroutine(StartBoostAfterDelay());
    }

    IEnumerator StartBoostAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeBoost);
        isBoosted = true;
        moveSpeed = boostedSpeed;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        while (isBoosted)
        {
            float originalSpeed = moveSpeed;

            // Резкое ускорение (рывок)
            moveSpeed = boostedSpeed * 1.8f;
            yield return new WaitForSeconds(dashDuration);

            // Резкое возвращение к обычному boostedSpeed
            moveSpeed = boostedSpeed;
            yield return new WaitForSeconds(dashInterval);
        }
    }

    public void EndReaction()
    {
        isReacting = false;
        isMoving = false;
        isBoosted = false;
    }
}
