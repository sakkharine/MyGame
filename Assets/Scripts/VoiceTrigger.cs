using UnityEngine;
using FMODUnity;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField]
    private EventReference voiceLine;

    private bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;

        if (other.CompareTag("Player"))
        {
            VoiceOverManager.Instance.PlayVoice(voiceLine);

            played = true;
        }
    }
}