using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowScript : MonoBehaviour
{
    [Header("�������� ���������")]
    [SerializeField] private Transform _objectToFollow;
    [SerializeField] private Vector2 _offset = Vector2.zero;
    [SerializeField] private float _smoothTime = 0.3f;

    [Header("��� �������� ������")]
    [SerializeField] public bool followX = true;
    [SerializeField] public bool followY = true;

    [Header("����������� ��� ������")]
    [SerializeField] private Vector2 _center = Vector2.zero;
    [SerializeField] private Vector2 _size = Vector2.one;

    [Header("Debug")]
    [SerializeField] private bool _showGizmos = true;

    private Vector3 _currentVelocity;
    private Rect _cameraZone;
    private Camera _camera;

    private Coroutine _zoomCoroutine;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        UpdateZoneRect();
    }

    public void Zoom(float targetSize, float duration)
    {
        if(_zoomCoroutine != null)
            StopCoroutine(_zoomCoroutine);

        _zoomCoroutine = StartCoroutine(ZoomCoroutine(targetSize, duration));
    }

    private IEnumerator ZoomCoroutine(float targetSize, float duration)
    {
        float elapsedTime = 0f;
        float startSize = _camera.orthographicSize;
        
        while(elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);

            yield return null;
        }
        
        UpdateZoneRect();
        
        _zoomCoroutine = null;
    }

    public void SetObjectToFollow(Transform target)
    {
        _objectToFollow = target;
    }
    
    private void UpdateZoneRect()
    {
        float height = _camera.orthographicSize * 2f;
        float width = height * _camera.aspect;
        Vector2 cameraRect = new Vector2(width, height);
        _cameraZone = new Rect(Vector2.zero, _size * 2f - cameraRect);
        _cameraZone.center = _center;


        if (_cameraZone.size.x < 0f)
        {
            _cameraZone.width = 0f;
            _cameraZone.center = _center;
        }


        if (_cameraZone.size.y < 0f)
        {
            _cameraZone.height = 0f;
            _cameraZone.center = _center;
        }
    }

    private void LateUpdate()
    {
        if (_objectToFollow == null)
            return;

        Vector2 targetPosition = _objectToFollow.position;
        targetPosition += _offset;

        Vector3 newTarget = transform.position;

        if (followX)
        {
            newTarget.x = Mathf.Clamp(targetPosition.x, _cameraZone.xMin, _cameraZone.xMax);
        }
            
        if (followY)
        {
            newTarget.y = Mathf.Clamp(targetPosition.y, _cameraZone.yMin, _cameraZone.yMax);
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            newTarget,
            ref _currentVelocity,
            _smoothTime
        );
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos)
            return;

        var color = Color.green;
        color.a = 0.1f;
        Gizmos.color = color;

        Gizmos.DrawWireCube(_cameraZone.center, _cameraZone.size);
    }

    private void OnValidate()
    {
        _camera = GetComponent<Camera>();
        UpdateZoneRect();
    }
}
