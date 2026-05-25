using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [System.Serializable]
    public class SceneKey
    {
        [Header("Key to press")]
        public KeyCode key;

        [Header("Scene name from Build Settings")]
        public string sceneName;
    }

    [Header("Scene switch list")]
    public SceneKey[] scenes;

    private static LevelSelector instance;

    private void Awake()
    {
        // DontDestroyOnLoad singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        foreach (SceneKey scene in scenes)
        {
            if (Input.GetKeyDown(scene.key))
            {
                LoadScene(scene.sceneName);
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty!");
        }
    }
}