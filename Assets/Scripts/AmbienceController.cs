using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AmbienceController : MonoBehaviour
{
    private EventInstance ambienceInstance;

    void Start()
    {
        if (FMODEvents.instance == null)
        {
            Debug.LogError("FMODEvents instance not found!");
            return;
        }

        ambienceInstance = RuntimeManager.CreateInstance(FMODEvents.instance.ambience);
        ambienceInstance.start();
    }

    private void OnDestroy()
    {
        if (ambienceInstance.isValid())
        {
            ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            ambienceInstance.release();
        }
    }
}
