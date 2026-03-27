using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    public UnityEvent OnCollect;

    public bool IsCollected { get; private set; } = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(IsCollected) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            IsCollected = true;
            OnCollect.Invoke();
        }
    }
}
