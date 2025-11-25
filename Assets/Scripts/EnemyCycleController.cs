using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class EnemyCycleController : MonoBehaviour
{
    [Header("Настройки логики")]
    [Tooltip("Имя параметра в Animator, который управляет фазой")]
    public string stateParameter = "state";

    [Tooltip("Общее количество фаз (1=Idle, 2=Attack, 3=Move)")]
    public int totalPhases = 3;

    [Tooltip("Задержка перед сменой состояния после касания")]
    public float reactionDelay = 0.5f;

    public float animationDelay = 0.5f;

    [Tooltip("Длительность анимации атаки (чтобы знать, когда перейти к следующей)")]
    public float attackDuration = 1.0f;

    [Tooltip("Длительность анимации движения (чтобы вернуться к Idle)")]
    public float moveDuration = 1.0f;

    [Tooltip("Фазы, во время которых враг смертелен")]
    public int[] deadlyPhases = { 2, 3 };

    [Tooltip("Тег игрока")]
    public string playerTag = "Player";

    [Header("Отладка")]
    public bool verbose = true;

    [Header("Состояния")]
    [Tooltip("Флаг: игрок касается врага")]
    public bool isTouched = false;

    private Animator anim;
    private int currentPhase = 1;
    private bool isReacting = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger(stateParameter, currentPhase);
        if (verbose) Debug.Log($"[{name}] Начальная фаза: {currentPhase}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag(playerTag)) return;

        isTouched = true; // <-- игрок коснулся врага

        // Проверка на смертельную фазу
        if (IsDeadlyPhase(currentPhase) && isReacting)
        {
            if (verbose) Debug.Log($"[{name}] Смертельное столкновение во фазе {currentPhase}");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // Если безопасная фаза — начинаем реакцию
        if (!isReacting)
        {
            StartCoroutine(HandlePhaseChange());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!enabled)
            return;

        if (!other.CompareTag(playerTag)) return;

        isTouched = true; // <-- продолжаем касание

        if (IsDeadlyPhase(currentPhase) && isReacting)
        {
            if (verbose) Debug.Log($"[{name}] Смертельное столкновение во фазе {currentPhase}");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        isTouched = false; // <-- игрок больше не касается
        if (verbose) Debug.Log($"[{name}] Игрок отошёл от врага");
    }

    IEnumerator HandlePhaseChange()
    {
        if (currentPhase == 1)
        {
            yield return new WaitForSeconds(reactionDelay);
            currentPhase = 2;
            anim.SetInteger(stateParameter, currentPhase);
            yield return new WaitForSeconds(animationDelay);
            isReacting = true;
            if (verbose) Debug.Log($"[{name}] Фаза {currentPhase} (атака)");
            yield return new WaitForSeconds(attackDuration);
            currentPhase = 3;
            anim.SetInteger(stateParameter, currentPhase);
            if (verbose) Debug.Log($"[{name}] Фаза {currentPhase} (движение)");
            yield return new WaitForSeconds(moveDuration);
            currentPhase = 1;
            anim.SetInteger(stateParameter, currentPhase);
            if (verbose) Debug.Log($"[{name}] Возврат к Idle");
            isReacting = false;
        }
    }

    private bool IsDeadlyPhase(int phase)
    {
        foreach (var deadly in deadlyPhases)
            if (deadly == phase)
                return true;
        return false;
    }
}
