using UnityEngine;
using FMODUnity;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField]
    private EventReference voiceLine;

    private bool played = false;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER HIT");
        if (played)
            return;
        Debug.Log("SOMETHING ENTERED");

        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            VoiceOverManager.Instance.PlayVoice(voiceLine);

            played = true;
        }
    }
}