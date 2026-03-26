using UnityEngine;
using DG.Tweening;

public class ShakeScaleEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;
    [SerializeField] private bool fadeOut = true;
    
    private Vector3 originalScale;
    private Tween shakeTween;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void Play()
    {
        KillTween();
        shakeTween = transform
            .DOShakeScale(duration, shakeStrength, shakeVibrato, shakeRandomness, fadeOut)
            .SetLoops(-1);
    }

    public void Stop()
    {
        KillTween();
        shakeTween = transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
    }

    private void KillTween()
    {
        if (shakeTween != null)
        {
            shakeTween.Kill();
            shakeTween = null;
        }
    }

    private void OnDestroy()
    {
        KillTween();
    }
}
