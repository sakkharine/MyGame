using UnityEngine;
using UnityEngine.Events;

public class TriggerOnStart : MonoBehaviour
{
    public UnityEvent OnStart;

    private void Start()
    {
        OnStart.Invoke();
    }
}
