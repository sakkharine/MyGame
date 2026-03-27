using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeEffect : MonoBehaviour
{
    [SerializeField] private float target = 0f;
    [SerializeField] private float duration = 1f;
    
    private CanvasGroup canvasGroup;

    private Tween tween;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Play()
    {
        tween = canvasGroup.DOFade(target, duration);
    }

    public IEnumerator PlayAsync()
    {
        yield return canvasGroup.DOFade(target, duration).WaitForCompletion();
    }

    public void Rewind()
    {
        tween.PlayBackwards();
    }
}
