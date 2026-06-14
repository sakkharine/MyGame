using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

[RequireComponent(typeof(Collider2D))]
public class TriggerSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneName;

    public float delayBeforeLoad = 0f;

    [Header("Player Settings")]
    public string triggeringTag = "Player";

    [Header("Karma Settings")]
    public bool ignoreKarma;
    public bool isAltWorld;

    [Header("Ending Settings")]
    public bool isSceneDependsOnKarma;

    public string goodKarmaScene;
    public string badKarmaScene;

    [SerializeField] private EventReference goodEndingVoice;
    [SerializeField] private EventReference badEndingVoice;

    [SerializeField] private SubtitleLine[] goodEndingSubtitles;
    [SerializeField] private SubtitleLine[] badEndingSubtitles;

    [Header("Debug")]
    public bool verbose = true;

    private bool isTriggered = false;
    private Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();

        if (myCollider == null)
        {
            Debug.LogError($"[TriggerSceneLoader] На объекте '{name}' нет Collider2D!");
            return;
        }

        if (!myCollider.isTrigger)
        {
            Debug.LogWarning($"[TriggerSceneLoader] Collider2D на объекте '{name}' не отмечен как IsTrigger.");
        }

        if (string.IsNullOrEmpty(sceneName) && !isSceneDependsOnKarma)
        {
            Debug.LogWarning($"[TriggerSceneLoader] Не указано имя сцены для '{name}'.");
        }

        if (verbose)
        {
            Debug.Log($"[TriggerSceneLoader] '{name}' готов.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (verbose)
        {
            Debug.Log($"[TriggerSceneLoader] '{other.name}' вошёл в триггер '{name}'.");
        }

        if (isTriggered)
            return;

        if (!other.CompareTag(triggeringTag))
            return;

        isTriggered = true;

        if (!ignoreKarma)
        {
            if (isAltWorld)
                KarmaCounter.AddToAltWorld();
            else
                KarmaCounter.AddToRealWorld();
        }

        if (delayBeforeLoad > 0f)
        {
            Invoke(nameof(LoadScene), delayBeforeLoad);
        }
        else
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (isSceneDependsOnKarma)
        {
            int altWorldScore = KarmaCounter.AltWorldScore;
            int goodWorldScore = KarmaCounter.RealWorldScore;

            if (altWorldScore > goodWorldScore)
            {
                StartCoroutine(
                    PlayEndingVoiceAndLoad(
                        badEndingVoice,
                        badEndingSubtitles,
                        badKarmaScene));
            }
            else
            {
                StartCoroutine(
                    PlayEndingVoiceAndLoad(
                        goodEndingVoice,
                        goodEndingSubtitles,
                        goodKarmaScene));
            }

            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("[TriggerSceneLoader] Имя сцены не указано.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayEndingVoiceAndLoad(
        EventReference voice,
        SubtitleLine[] subtitles,
        string sceneToLoad)
    {
        VoiceOverManager.Instance.ClearQueue();

        yield return null;

        VoiceOverManager.Instance.PlayVoice(voice);

        if (subtitles != null && subtitles.Length > 0)
        {
            StartCoroutine(PlaySubtitles(subtitles));
        }

        yield return null;

        while (VoiceOverManager.Instance.IsPlaying)
        {
            yield return null;
        }

        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.ClearSubtitle();
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator PlaySubtitles(SubtitleLine[] subtitles)
    {
        float elapsed = 0f;

        foreach (SubtitleLine line in subtitles)
        {
            while (elapsed < line.time)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (SubtitleManager.Instance != null)
            {
                SubtitleManager.Instance.SetSubtitle(line.text);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        var col = GetComponent<Collider2D>();

        if (col == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}