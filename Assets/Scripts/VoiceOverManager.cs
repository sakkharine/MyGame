using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using FMODUnity;
using FMOD.Studio;

public class VoiceOverManager : MonoBehaviour
{
    private static VoiceOverManager instance;
    public static VoiceOverManager Instance => instance;

    private Queue<EventReference> voiceQueue = new();

    private EventInstance currentVoice;

    private bool isPlaying = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearQueue();
    }

    public void PlayVoice(EventReference voiceEvent)
    {
        voiceQueue.Enqueue(voiceEvent);

        if (!isPlaying)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isPlaying = true;

        while (voiceQueue.Count > 0)
        {
            EventReference nextVoice = voiceQueue.Dequeue();

            currentVoice = RuntimeManager.CreateInstance(nextVoice);

            currentVoice.start();

            PLAYBACK_STATE state;

            do
            {
                currentVoice.getPlaybackState(out state);

                yield return null;

            } while (state != PLAYBACK_STATE.STOPPED);

            currentVoice.release();
        }

        isPlaying = false;
    }

    public void ClearQueue()
    {
        voiceQueue.Clear();

        if (currentVoice.isValid())
        {
            currentVoice.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentVoice.release();
        }

        isPlaying = false;
    }
}