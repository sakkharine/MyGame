using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicController : MonoBehaviour
{
    private EventInstance musicInstance;

    void Start()
    {
        // берём ссылку из FMODEvents (заполненной в инспекторе)
        musicInstance = RuntimeManager.CreateInstance(FMODEvents.instance.music);
        musicInstance.start();
    }

    public void SetDrums(bool on)
    {
        musicInstance.setParameterByName("Drums", on ? 1f : 0f);
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

}
