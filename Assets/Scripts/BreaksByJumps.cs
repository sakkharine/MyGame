using UnityEngine;

public class BreaksByJumps : MonoBehaviour
{

    [SerializeField] int timesToJump;

    private int jumpCounter = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.TryGetComponent(out Rigidbody2D rb))
        {
            //Если объект падает вниз
            if (Vector3.Dot(collision.relativeVelocity, Vector3.down) >= 0)
            {
                IncreaseJumpCounter();
            }
        }
    }

    private void IncreaseJumpCounter()
    {
        jumpCounter++;

        if (timesToJump <= jumpCounter)
        {
            Destroy(gameObject);
        }
    }
}
