using UnityEngine;

public class ForceMovePlayer : MonoBehaviour
{
    [SerializeField] public float Force;
    [SerializeField] public Vector2 Direction;
    [SerializeField] private Rigidbody2D _player;
    
    private void FixedUpdate()
    {
        _player.AddForce(_player.mass * Force * Direction);    
    }
}
