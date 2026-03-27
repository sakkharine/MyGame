using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float period = 2f;
    
    private Vector3 startPosition;
    private float time;

    private void Start()
    {
        startPosition = transform.localPosition;
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        float yOffset = Mathf.Sin(time * 2f * Mathf.PI / period) * amplitude;
        transform.localPosition = startPosition + Vector3.up * yOffset;
    }
}
