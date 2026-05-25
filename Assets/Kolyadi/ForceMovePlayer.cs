using UnityEngine;

public class ForceMovePlayer : MonoBehaviour
{
    [SerializeField] public float Force;
    [SerializeField] public Vector2 Direction;
    [SerializeField] private characterController _player;

    private void Update()
    {
        _player.SecondaryVelocity = Force * Direction;
    }

    private void OnEnable()
    {
        _player.SecondaryVelocity = Force * Direction;
    }
    
    private void OnDisable()
    {
        _player.SecondaryVelocity = Vector2.zero;
    }

    private void OnValidate()
    {
        if(_player != null)
            _player.SecondaryVelocity = Force * Direction;
    }
}
