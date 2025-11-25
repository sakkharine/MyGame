using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZoneByTime : MonoBehaviour
{
    [SerializeField] float time = 1f;

    Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coroutine = StartCoroutine(WaitForDeath());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine(coroutine);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
