using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] string firstSceneName;

    private void Awake()
    {
        playButton.onClick.AddListener(LoadNeededScene);
    }

    private void LoadNeededScene()
    {
        SceneManager.LoadScene(firstSceneName);
    }
}
