using System.Collections;
using UnityEngine;

public class Kochka : MonoBehaviour
{
    public enum KochkaType { Instant, Delayed } // тип кочки
    [SerializeField] KochkaType type = KochkaType.Delayed;

    [SerializeField] float duration = 1f; // время исчезновения для плавного типа

    SpriteRenderer spriteRenderer;
    Collider2D coll;

    Coroutine fallCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    IEnumerator FallRoutine()
    {
        coll.enabled = false;

        if (type == KochkaType.Instant)
        {
            Destroy(gameObject);
            yield break;
        }

        Color color = spriteRenderer.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / duration;
            spriteRenderer.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (fallCoroutine == null && gameObject.activeSelf)
            fallCoroutine = StartCoroutine(FallRoutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
