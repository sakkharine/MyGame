using System.Collections;
using QuestGame;
using UnityEngine;
using UnityEngine.Events;

public class InteractableWithTimer : Interactable
{
    public float interactionTime = 5f;
    public UnityEvent OnCancelInteract;
    public UnityEvent OnFinishInteract;
    
    private Coroutine interactionCoroutine;

    public override void Activate()
    {
        if (IsInteractable && !IsLocked)
        {
            OnBeginInteract.Invoke();
            interactionCoroutine = StartCoroutine(InteractAfterTime());
        }
    }

    public void Cancel()
    {
        OnCancelInteract.Invoke();
        if (interactionCoroutine != null)
        {
            StopCoroutine(interactionCoroutine);
        }
    }

    private IEnumerator InteractAfterTime()
    {
        yield return new WaitForSeconds(interactionTime);
        IsInteractable = false;
        OnFinishInteract.Invoke();
    }
}