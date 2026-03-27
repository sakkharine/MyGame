using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeDestroyEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    
    private SpriteRenderer spriteRenderer;
    private Tween fadeTween;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Play()
    {
        KillTween();
        
        fadeTween = spriteRenderer.DOFade(0f, duration).OnComplete(() => gameObject.SetActive(false));
    }

    private void KillTween()
    {
        if (fadeTween != null)
        {
            fadeTween.Kill();
            fadeTween = null;
        }
    }

    private void OnDestroy()
    {
        KillTween();
    }
}
