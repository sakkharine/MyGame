using Unity.VisualScripting;
using UnityEngine;

public class ButtonRotate : MonoBehaviour
{
    public RingRotator targetRing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetRing.StartRotation();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetRing.StopRotation();
        }
    }
}
