using UnityEngine;

public class FlyAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out characterController character))
        {
            character.GiveFlyAbility();
            Debug.Log("FlyAbility triggered!");
            Destroy(gameObject, 0.1f); 
        }
    }

}
