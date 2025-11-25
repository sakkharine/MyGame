using UnityEngine;
using UnityEngine.SceneManagement;

public class StoneBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    public bool IsDangerous { get; private set; } = false;
    private bool _wasActivated = false;

    private void Awake()
    {
        _rigidbody.simulated = false;
        IsDangerous = true;
    }
    public void Activate()
    {
        if (_wasActivated)
            return;

        _rigidbody.simulated = true;
        _wasActivated = true;
        IsDangerous = true;
    }

    public void Deactivate()
    {
        _rigidbody.simulated = true;
        _wasActivated = true;
        IsDangerous = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && IsDangerous)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
