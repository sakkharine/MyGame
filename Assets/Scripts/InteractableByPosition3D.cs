using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableByPosition3D : Interactable
{
    [SerializeField] private float distance = 1f;
    
    public Transform targetTransform;

    public void StartTracking(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        if (targetTransform != null && IsInteractable)
        {
            if (Vector3.Distance(transform.position, targetTransform.position) < distance)
            {
                OnBeginInteract.Invoke();
                IsInteractable = false;
            }
        }
    }
}
