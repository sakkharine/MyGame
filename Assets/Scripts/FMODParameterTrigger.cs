using UnityEngine;
using System.Collections;

public class FMODParameterTrigger : MonoBehaviour
{
    public string parameterName = "Drums";
    public float value = 1f;

    [Header("Fade Settings")]
    public bool useFade = false;
    public float fadeTime = 1f;

    private MusicController music;
    private bool isFading = false;

    void Start()
    {
        music = MusicController.Instance;  

        if (music == null)
        {
            Debug.LogError("[FMODParameterTrigger] ERROR: MusicController INSTANCE is NULL!");
        }
        else
        {
            Debug.Log("[FMODParameterTrigger] MusicController assigned via Singleton");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (music == null)
        {
            Debug.LogError("[FMODParameterTrigger] Cannot set parameter — music == NULL");
            return;
        }

        Debug.Log("[FMODParameterTrigger] Triggered → " + parameterName + " = " + value);

        if (useFade)
        {
            StartCoroutine(FadeParameter());
        }
        else
        {
            music.SetParameter(parameterName, value);
        }
    }

    private IEnumerator FadeParameter()
    {
        if (isFading) yield break;
        isFading = true;

        float startValue = 0f;
        music.GetMusicInstance().getParameterByName(parameterName, out startValue);

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            float lerpValue = Mathf.Lerp(startValue, value, timer / fadeTime);
            music.SetParameter(parameterName, lerpValue);

            yield return null;
        }

        music.SetParameter(parameterName, value);

        Debug.Log("[FMODParameterTrigger] Fade complete: " + parameterName + " = " + value);

        isFading = false;
    }
}
