using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class EnemyTouchDetector : MonoBehaviour
{
    [Header("Настройки")]
    public string playerTag = "Player";

    [Header("Animator")]
    public string animatorBoolName = "isTouched"; // имя параметра в Animator

    [Header("Отладка")]
    public bool verbose = true;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        anim.SetBool(animatorBoolName, true); // обновляем параметр Animator
        if (verbose) Debug.Log($"{name}: Игрок вошёл в коллайдер");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        anim.SetBool(animatorBoolName, true); // поддерживаем true
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        anim.SetBool(animatorBoolName, false); // игрок вышел
        if (verbose) Debug.Log($"{name}: Игрок вышел из коллайдера");
    }
}
