using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private UIFadeEffect fadeEffect;
    
    public void LoadScene()
    {
        StartCoroutine(LoadingRoutine());
    }

    public IEnumerator LoadingRoutine()
    {
        yield return fadeEffect.PlayAsync();
        
        SceneManager.LoadScene(sceneName);
    }
}
