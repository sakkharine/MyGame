using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private EventReference voiceLine;

    [SerializeField]
    private SubtitleLine[] subtitles;

    [SerializeField]
    private SubtitlePicture[] pictures;

    public Image image;

    public string sceneAfterIntro;

    private void Start()
    {
        StartCoroutine(PlayVoiceRoutine());
    }

    private IEnumerator PlayVoiceRoutine()
    {
        if (VoiceOverManager.Instance == null)
        {
            Debug.LogError("VoiceOverManager is NULL");
            yield break;
        }

        VoiceOverManager.Instance.PlayVoice(voiceLine);

        if (subtitles != null && subtitles.Length > 0)
            StartCoroutine(PlaySubtitles());

        StartCoroutine(PlayPictures());
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
        
        SceneManager.LoadScene(sceneAfterIntro);
    }

    private IEnumerator PlayPictures()
    {
        float elapsed = 0f;

        foreach (SubtitlePicture line in pictures)
        {
            while (elapsed < line.time)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            image.sprite = line.sprite;
        }

        yield return new WaitForSeconds(3f);
    }

    [Serializable]
    public class SubtitlePicture
    {
        public float time;
        public Sprite sprite;
    }
}
