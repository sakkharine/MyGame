using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Animator), typeof(Collider2D))]
public class EnemyCycleDisabler : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Имя флага в Animator, при включении которого EnemyCycleController отключается")]
    public string insideFlagName = "insideActive";

    [Tooltip("Имя флага в Animator, который активен при касании игрока")]
    public string touchFlagName = "isTouched";

    [Tooltip("Тег игрока")]
    public string playerTag = "Player";

    [Tooltip("Ссылка на EnemyCycleController (если не указано, ищется автоматически)")]
    public EnemyCycleController enemyCycle;

    [Header("Состояния")]
    [Tooltip("Флаг: игрок касается врага")]
    public bool isTouched = false;

    [Header("Отладка")]
    public bool verbose = true;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (enemyCycle == null)
            enemyCycle = GetComponent<EnemyCycleController>();
    }

    void Update()
    {
        if (enemyCycle == null) return;

        bool inside = anim.GetBool(insideFlagName);

        if (enemyCycle.enabled && inside)
        {
            enemyCycle.enabled = false;
            if (verbose) Debug.Log($"{name}: EnemyCycleController ОТКЛЮЧЁН (insideActive = true)");
        }
        else if (!enemyCycle.enabled && !inside)
        {
            enemyCycle.enabled = true;
            if (verbose) Debug.Log($"{name}: EnemyCycleController ВКЛЮЧЁН (insideActive = false)");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        isTouched = true;
        anim.SetBool(touchFlagName, true);

        if (verbose) Debug.Log($"{name}: Игрок вошёл в коллайдер (isTouched = true)");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        isTouched = true;
        anim.SetBool(touchFlagName, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        isTouched = false;
        anim.SetBool(touchFlagName, false);

        if (verbose) Debug.Log($"{name}: Игрок вышел из коллайдера (isTouched = false)");
    }
}
