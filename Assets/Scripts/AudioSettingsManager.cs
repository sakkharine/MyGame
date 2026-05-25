using UnityEngine;
using FMODUnity;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    private const string MutePrefKey = "AudioMuted";

    private bool isMuted;

    public bool IsMuted => isMuted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        isMuted = PlayerPrefs.GetInt(MutePrefKey, 0) == 1;
        ApplyMute();
    }

    public void SetMute(bool mute)
    {
        isMuted = mute;
        ApplyMute();
        PlayerPrefs.SetInt(MutePrefKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleMute()
    {
        SetMute(!isMuted);
    }

    private void ApplyMute()
    {
        RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup masterGroup);
        masterGroup.setVolume(isMuted ? 0f : 1f);
    }
}
