using UnityEngine;

public class Patroller : MonoBehaviour
{
    public Vector3 Destination;

    public float speed;

    private Vector3 _startPosition;
    private float _t = 0f;
    private int _direction = 1;

    private void Reset()
    {
        Destination = transform.position;
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _t += Time.deltaTime * speed * _direction;

        if (_t >= 1f)
        {
            _t = 1f;
            _direction = -1;
            Flip();
        }
        else if (_t <= 0f)
        {
            _t = 0f;
            _direction = 1;
            Flip(); 
        }

        float distance = Vector3.Distance(_startPosition, Destination);
        float adjustedT = distance > 0f ? _t : 0f;

        transform.position = Vector3.Lerp(_startPosition, Destination, adjustedT);
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
