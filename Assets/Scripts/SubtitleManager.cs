using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;

    private const string SubtitlesPrefKey = "SubtitlesEnabled";

    [SerializeField]
    private TextMeshProUGUI subtitleText;

    private bool subtitlesEnabled;

    public bool SubtitlesEnabled => subtitlesEnabled;

    private void Awake()
    {
        Instance = this;
        subtitlesEnabled = PlayerPrefs.GetInt(SubtitlesPrefKey, 1) == 1;
    }

    public void SetEnabled(bool enabled)
    {
        subtitlesEnabled = enabled;
        PlayerPrefs.SetInt(SubtitlesPrefKey, enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (!enabled)
            subtitleText.text = "";
    }

    public void SetSubtitle(string text)
    {
        if (!subtitlesEnabled) return;
        subtitleText.text = text;
    }

    public void ClearSubtitle()
    {
        subtitleText.text = "";
    }
}
