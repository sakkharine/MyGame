using System.Collections;
using UnityEngine;
using FMODUnity;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField]
    private EventReference voiceLine;

    [SerializeField]
    private SubtitleLine[] subtitles;

    private bool played = false;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER HIT");
        if (played)
            return;
        Debug.Log("SOMETHING ENTERED");

        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            VoiceOverManager.Instance.PlayVoice(voiceLine);

            if (subtitles != null && subtitles.Length > 0)
                StartCoroutine(PlaySubtitles());

            played = true;
        }
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