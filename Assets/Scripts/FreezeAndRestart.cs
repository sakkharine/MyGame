using UnityEngine;
using UnityEngine.SceneManagement;

public class FreezeAndRestartZone : MonoBehaviour
{
    [Header("Настройки зоны")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Настройки задержек")]
    public float freezeDelay = 0.5f;   // задержка перед заморозкой
    public float restartDelay = 120f;  // 2 минуты

    [Header("Игрок")]
    public Rigidbody2D playerRb;
    public Animator playerAnimator;

    private bool isActivated = false;

    private void Update()
    {
        if (isActivated) return;

        // Проверяем, что игрок в зоне
        if (playerRb.transform.position.x > minX &&
            playerRb.transform.position.x < maxX &&
            playerRb.transform.position.y > minY &&
            playerRb.transform.position.y < maxY)
        {
            // Проверяем нажатие влево
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(FreezeAndRestart());
                isActivated = true;
            }
        }
    }

    private System.Collections.IEnumerator FreezeAndRestart()
    {
        // Ждём 0.5 секунды перед заморозкой
        yield return new WaitForSeconds(freezeDelay);

        // Ставим state = 0
        if (playerAnimator != null)
        {
            playerAnimator.SetInteger("state", 0);
        }

        // Полная заморозка движения
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.simulated = false;
        }

        // Ждём 2 минуты и перезапускаем сцену
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- ГИЗМО ДЛЯ ВИДИМОСТИ ЗОНЫ ---
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f); // полупрозрачный оранжевый
        Gizmos.DrawCube(
            new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0),
            new Vector3((maxX - minX), (maxY - minY), 0.2f)
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0),
            new Vector3((maxX - minX), (maxY - minY), 0.2f)
        );
    }
}
