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

    private Coroutine queueCoroutine;

    private bool isPlaying = false;
public bool IsPlaying => isPlaying;
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

        Debug.Log("[VoiceOver] Added to queue");

        if (!isPlaying)
        {
            queueCoroutine = StartCoroutine(ProcessQueue());
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

            Debug.Log("[VoiceOver] Playing voice");

            PLAYBACK_STATE state;

            do
            {
                currentVoice.getPlaybackState(out state);

                yield return null;

            } while (state != PLAYBACK_STATE.STOPPED);

            currentVoice.release();
        }

        isPlaying = false;

        queueCoroutine = null;
    }

    public void ClearQueue()
    {
        Debug.Log("[VoiceOver] CLEAR QUEUE");

        voiceQueue.Clear();

        if (queueCoroutine != null)
        {
            StopCoroutine(queueCoroutine);
            queueCoroutine = null;
        }

        if (currentVoice.isValid())
        {
            PLAYBACK_STATE state;
            currentVoice.getPlaybackState(out state);

            Debug.Log("[VoiceOver] STOPPING CURRENT VOICE: " + state);

            currentVoice.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            currentVoice.clearHandle();
        }

        isPlaying = false;
    }
}