using System.Collections;
using UnityEngine;

public class Kochka : MonoBehaviour
{
    public enum KochkaType { Instant, Delayed }
    [SerializeField] KochkaType type = KochkaType.Delayed;

    [SerializeField] float duration = 1f;
    [SerializeField] float destroyDelay = 1f;

    private float destroyTimer = 0f;

    public bool IsDestroying => destroyTimer >= destroyDelay;

    SpriteRenderer spriteRenderer;
    Collider2D coll;

    Coroutine destroyCoroutine;

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

    IEnumerator DestroyRoutine()
    {
        while (destroyTimer < destroyDelay)
        {
            destroyTimer += Time.deltaTime;
            yield return null;
        }

        yield return FallRoutine();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (destroyCoroutine == null && !IsDestroying)
            destroyCoroutine = StartCoroutine(DestroyRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
