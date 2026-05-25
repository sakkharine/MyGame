using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button exitButton;
    [SerializeField] Toggle muteToggle;
    [SerializeField] Toggle subtitleToggle;
    [SerializeField] GameObject menu;

    bool isShown = false;

    private void Awake()
    {
        Hide();
        continueButton.onClick.AddListener(Hide);
        exitButton.onClick.AddListener(GoToMenu);

        if (muteToggle != null)
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        if (subtitleToggle != null)
            subtitleToggle.onValueChanged.AddListener(OnSubtitleToggleChanged);
    }

    private void OnEnable()
    {
        if (AudioSettingsManager.Instance != null)
            muteToggle.SetIsOnWithoutNotify(!AudioSettingsManager.Instance.IsMuted);

        if (SubtitleManager.Instance != null)
            subtitleToggle.SetIsOnWithoutNotify(SubtitleManager.Instance.SubtitlesEnabled);
    }

    private void OnMuteToggleChanged(bool isOn)
    {
        if (AudioSettingsManager.Instance != null)
            AudioSettingsManager.Instance.SetMute(!isOn);
    }

    private void OnSubtitleToggleChanged(bool isOn)
    {
        if (SubtitleManager.Instance != null)
            SubtitleManager.Instance.SetEnabled(isOn);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
        isShown = true;
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        isShown = false;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isShown)
                Show();
            else
                Hide();
        }
    }
}
