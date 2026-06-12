using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class TriggerSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Имя сцены, на которую нужно перейти")]
    public string sceneName;

    [Tooltip("Задержка перед переходом (в секундах)")]
    public float delayBeforeLoad = 0f;

    [Header("Player Settings")]
    [Tooltip("Тег объекта, который активирует триггер (обычно 'Player')")]
    public string triggeringTag = "Player";

    [Header("Karma Settings")]
    public bool ignoreKarma;
    [Tooltip("Если true — переход засчитывается в карму Альтернативного мира, иначе — Настоящего")]
    public bool isAltWorld;


    [Header("Karma Settings")] 
    public bool isSceneDependsOnKarma;

    public string goodKarmaScene;
    public string equalKarmaScene;
    public string badKarmaScene;
    
    [Header("Debug")]
    [Tooltip("Если true — будут подробные логи в консоли")]
    public bool verbose = true;

    private bool isTriggered = false;
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();

        if (myCollider == null)
        {
            Debug.LogError($"[TriggerSceneLoader]  На объекте '{name}' нет Collider2D!");
            return;
        }

        if (!myCollider.isTrigger)
        {
            Debug.LogWarning($"[TriggerSceneLoader]  Collider2D на объекте '{name}' не отмечен как IsTrigger. Без этого триггер не сработает.");
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning($"[TriggerSceneLoader]  В инспекторе не указано имя сцены для '{name}'.");
        }

        if (verbose)
        {
            Debug.Log($"[TriggerSceneLoader]  '{name}' готов. sceneName='{sceneName}', delay={delayBeforeLoad}, triggeringTag='{triggeringTag}', isTrigger={myCollider.isTrigger}");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (verbose)
            Debug.Log($"[TriggerSceneLoader]  OnTriggerEnter2D: '{other.name}' (tag: {other.tag}) вошёл в триггер '{name}'.");

        if (isTriggered) return;

        if (!other.CompareTag(triggeringTag))
        {
            if (verbose) Debug.Log($"[TriggerSceneLoader]  Объект '{other.name}' не имеет тег '{triggeringTag}'.");
            return;
        }

        isTriggered = true;
        
        if(!ignoreKarma)
        {
            if (isAltWorld)
                KarmaCounter.AddToAltWorld();
            else
                KarmaCounter.AddToRealWorld();
        }
        
        if (delayBeforeLoad > 0f)
        {
            if (verbose) Debug.Log($"[TriggerSceneLoader]  Переход на сцену '{sceneName}' через {delayBeforeLoad} секунд...");
            Invoke(nameof(LoadScene), delayBeforeLoad);
        }
        else
        {
            if (verbose) Debug.Log($"[TriggerSceneLoader] ▶Переход на сцену '{sceneName}' сразу.");
            LoadScene();
        }
    }

    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[TriggerSceneLoader]  Имя сцены не указано! Укажи sceneName в инспекторе.");
            return;
        }

        if (verbose)
            Debug.Log($"[TriggerSceneLoader] Загружаем сцену '{sceneName}'...");

        if (isSceneDependsOnKarma)
        {
            int altWorldScore = KarmaCounter.AltWorldScore;
            int goodWorldScore = KarmaCounter.RealWorldScore;
            
            if (altWorldScore > goodWorldScore)
            {
                SceneManager.LoadScene(badKarmaScene);
            }
            else if(altWorldScore < goodWorldScore)
            {
                SceneManager.LoadScene(goodWorldScore);
            }
            else
            {
                SceneManager.LoadScene(equalKarmaScene);
            }
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    // Визуализация границ триггера
    private void OnDrawGizmosSelected()
    {
        var col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}
