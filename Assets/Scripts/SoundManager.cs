using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public EnvironmentSounds currentSounds;

    private AudioSource audioSrc;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        audioSrc = GetComponent<AudioSource>();
    }

    public static void Play(string soundName, float volume = 1f)
    {
        if (Instance == null)
        {
            Debug.LogError("SoundManager not in scene!");
            return;
        }

        AudioClip clip = Instance.GetClipByName(soundName);

        if (clip != null)
            Instance.audioSrc.PlayOneShot(clip, volume);
        else
            Debug.LogWarning("Sound '" + soundName + "' not found");
    }

    private AudioClip GetClipByName(string name)
    {
        switch (name)
        {
            case "Jump": return currentSounds.jump;
            case "Hit": return currentSounds.hit;
            case "Pickup": return currentSounds.pickup;

            default:
                return null;
        }
    }

    // Рандомные группы  
    public static void PlayRandom(string groupName, float volume = 1f)
    {
        if (Instance == null) return;

        AudioClip[] arr = Instance.GetRandomGroup(groupName);
        if (arr == null || arr.Length == 0) return;

        AudioClip clip = arr[Random.Range(0, arr.Length)];
        Instance.audioSrc.PlayOneShot(clip, volume);
    }

    private AudioClip[] GetRandomGroup(string name)
    {
        switch (name)
        {
            case "Footstep": return currentSounds.footsteps;
            case "GrassStep": return currentSounds.grassSteps;
            case "RockStep": return currentSounds.rockSteps;

            default:
                return null;
        }
    }
}
