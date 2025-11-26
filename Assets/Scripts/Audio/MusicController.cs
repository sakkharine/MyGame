using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;
    private EventInstance musicInstance;
    private static bool isInitialized = false; // static, чтобы сохранялось между сценами

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

            Debug.Log("[MusicController] Music started and will persist across scenes");
        }
        else
        {
            Debug.Log("[MusicController] Already initialized, not starting music again");
        }
    }

    public void SetParameter(string name, float value)
    {
        musicInstance.setParameterByName(name, value);
    }

    public EventInstance GetMusicInstance()
    {
        return musicInstance;
    }
}
