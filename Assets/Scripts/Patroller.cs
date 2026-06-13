using UnityEngine;

public class Patroller : MonoBehaviour
{
    public Vector3 Destination;

    public float speed;

    private Vector3 _startPosition;
    private Vector3 _direction;
    private bool _goingToDestination = true;

    private void Reset()
    {
        Destination = transform.position;
    }

    private void Start()
    {
        _startPosition = transform.position;
        _direction = (Destination - _startPosition).normalized;
    }

    private void Update()
    {
        transform.position += speed * _direction * Time.deltaTime;

        Vector3 target = _goingToDestination ? Destination : _startPosition;

        if (Vector3.Dot(target - transform.position, _direction) <= 0f)
        {
            transform.position = target;
            _direction = -_direction;
            _goingToDestination = !_goingToDestination;
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Destination, 0.3f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, Destination);
    }
}
