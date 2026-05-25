using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;
    public static MusicController Instance => instance;  

    private EventInstance musicInstance;
    private static bool isInitialized = false;

    private void Awake()
    {
        print("Awake");
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        print("Awake 2");

        instance = this;
        DontDestroyOnLoad(gameObject);
        print("Awake 3");
    }

    private System.Collections.IEnumerator Start()
    {
        yield return null;

        if (!isInitialized)
        {
            RuntimeManager.LoadBank("Master", true);
            RuntimeManager.LoadBank("Master.strings", true);
            RuntimeManager.LoadBank("VO", true);

            musicInstance = RuntimeManager.CreateInstance("event:/Music/Monster");
            musicInstance.start();
            //musicInstance = RuntimeManager.CreateInstance(FMODEvents.instance.music);
            //musicInstance.start();
            isInitialized = true;
            Debug.Log("[MusicController] Music STARTED");
        }
    }

    public void SetParameter(string name, float value)
    {
        musicInstance.setParameterByName(name, value);
        Debug.Log("[MusicController] PARAM → " + name + " = " + value);
    }

    public EventInstance GetMusicInstance()
    {
        return musicInstance;
    }
}
