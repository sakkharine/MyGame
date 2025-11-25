using UnityEngine;

public class StoneActivationTrigger : MonoBehaviour
{
    [SerializeField] private StoneBehaviour _stone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _stone.Activate();
        }
    }
}
