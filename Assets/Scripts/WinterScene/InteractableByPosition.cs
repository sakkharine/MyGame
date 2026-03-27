using System;
using UnityEngine;

public class InteractableByPosition : Interactable
{
    [SerializeField] private float distance = 1f;
    
    private Transform targetTransform;

    public void StartTracking(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        if (targetTransform != null && IsInteractable)
        {
            if (Mathf.Abs(transform.position.x - targetTransform.position.x) < distance)
            {
                OnBeginInteract.Invoke();
                IsInteractable = false;
            }
        }
    }
}
