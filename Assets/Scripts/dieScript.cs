using UnityEngine;
using UnityEngine.SceneManagement;

public class dieScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        // Перезагружаем уровень, когда любой объект входит в триггер
        Debug.Log("Триггер сработал! Перезагрузка уровня...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}