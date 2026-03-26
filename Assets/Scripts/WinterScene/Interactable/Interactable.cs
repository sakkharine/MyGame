using QuestGame;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnBeginInteract;
    
    public bool IsInteractable { get; protected set; } = true;

    public bool IsLocked
    {
        get
        {
            if (locks != null)
            {
                foreach (var lockCondition in locks)
                {
                    if (lockCondition.IsLocked)
                        return true;
                }
            }
            
            return false;
        }
    }
    
    private ILock[] locks;

    private void Awake()
    {
        locks = GetComponents<ILock>();
    }

    public virtual void Activate()
    {
        if (IsInteractable && !IsLocked)
        {
            OnBeginInteract.Invoke();
            IsInteractable = false;
        }
    }
}
