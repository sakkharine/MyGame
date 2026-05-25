using System.Collections;
using UnityEngine;
using FMODUnity;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField]
    private EventReference voiceLine;

    [SerializeField]
    private bool clearQueueBeforePlay = false;

    [SerializeField]
    private bool playOnlyOnce = true;

    [SerializeField]
    private SubtitleLine[] subtitles;

    private bool played = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playOnlyOnce && played)
            return;

        if (!other.CompareTag("Player"))
            return;

        StartCoroutine(PlayVoiceRoutine());

        played = true;
    }

    private IEnumerator PlayVoiceRoutine()
    {
        if (VoiceOverManager.Instance == null)
        {
            Debug.LogError("VoiceOverManager is NULL");
            yield break;
        }

        if (clearQueueBeforePlay)
        {
            VoiceOverManager.Instance.ClearQueue();

            yield return null;
        }

        VoiceOverManager.Instance.PlayVoice(voiceLine);

        if (subtitles != null && subtitles.Length > 0)
            StartCoroutine(PlaySubtitles());
    }

    private IEnumerator PlaySubtitles()
    {
        float elapsed = 0f;

        foreach (SubtitleLine line in subtitles)
        {
            while (elapsed < line.time)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            SubtitleManager.Instance.SetSubtitle(line.text);
        }

        yield return new WaitForSeconds(3f);
        SubtitleManager.Instance.ClearSubtitle();
    }
}