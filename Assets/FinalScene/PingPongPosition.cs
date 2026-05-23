using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PingPongPosition : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private bool useRandomSeed = true;
    
    private Vector3 _startPosition;
    private float _randomSeed;
    
    private void Awake()
    {
        if (useRandomSeed)
        {
            _randomSeed = Random.Range(-2f * Mathf.PI, 2f * Mathf.PI);
        }
    }

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(_randomSeed + Time.time * speed) * amplitude;
        transform.position = _startPosition + new Vector3(offset, 0f, 0f);
    }
}
