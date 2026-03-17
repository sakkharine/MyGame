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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!isInitialized)
        {
            musicInstance = RuntimeManager.CreateInstance(FMODEvents.instance.music);
            musicInstance.start();
            isInitialized = true;

            Debug.Log("[MusicController] Music STARTED (Singleton, persistent)");
        }
        else
        {
            Debug.Log("[MusicController] Already initialized — NOT restarting music");
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
