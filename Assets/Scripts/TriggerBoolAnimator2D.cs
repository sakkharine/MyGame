using UnityEngine;

public class TriggerBoolAnimator2D : MonoBehaviour
{
    [Header("Target Animator (если пусто — берётся с этого объекта)")]
    public Animator targetAnimator;

    [Header("Animator Bool Parameter")]
    public string parameterName = "isTouched";

    [Header("Values To Set")]
    public bool valueOnEnter = true;
    public bool valueOnStay = true;
    public bool valueOnExit = false;

    [Header("Enable / Disable Events")]
    public bool useEnter = true;
    public bool useStay = false;
    public bool useExit = true;

    private void Awake()
    {
        // Автопоиск аниматора
        if (targetAnimator == null)
            targetAnimator = GetComponent<Animator>();

        if (targetAnimator == null)
            Debug.LogError($"[TriggerBoolAnimator2D] ❌ Animator не найден на {gameObject.name}");
        else
            Debug.Log($"[TriggerBoolAnimator2D] ✔ Animator найден на {gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TriggerBoolAnimator2D] Enter by {other.name}");

        if (!useEnter) return;
        if (!other.CompareTag("Player")) return;

        if (targetAnimator)
        {
            targetAnimator.SetBool(parameterName, valueOnEnter);
            Debug.Log($" → SetBool({parameterName}) = {valueOnEnter}");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"[TriggerBoolAnimator2D] Stay by {other.name}");

        if (!useStay) return;
        if (!other.CompareTag("Player")) return;

        if (targetAnimator)
        {
            targetAnimator.SetBool(parameterName, valueOnStay);
            Debug.Log($" → SetBool({parameterName}) = {valueOnStay}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[TriggerBoolAnimator2D] Exit by {other.name}");

        if (!useExit) return;
        if (!other.CompareTag("Player")) return;

        if (targetAnimator)
        {
            targetAnimator.SetBool(parameterName, valueOnExit);
            Debug.Log($" → SetBool({parameterName}) = {valueOnExit}");
        }
    }
}
