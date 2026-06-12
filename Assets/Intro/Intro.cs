using FMODUnity;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private EventReference[] voiceLines;

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
        if (subtitles != null && subtitles.Length > 0)
            StartCoroutine(PlaySubtitles());

        StartCoroutine(PlayPictures());

        foreach (EventReference voiceLine in voiceLines)
        {
            EventInstance instance = RuntimeManager.CreateInstance(voiceLine);
            instance.start();

            PLAYBACK_STATE state;
            do
            {
                instance.getPlaybackState(out state);
                yield return null;
            } while (state != PLAYBACK_STATE.STOPPED);

            instance.release();
            
            yield return new WaitForSeconds(3f);
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
