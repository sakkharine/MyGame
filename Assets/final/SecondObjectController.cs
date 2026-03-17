using UnityEngine;
using System.Collections;

public class SecondObjectController : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem effectParticles; // ссылка на ParticleSystem
    public float delayBeforeParticles = 1f; // задержка перед запуском частиц

    // Этот метод вызывается Animation Event в конце второй анимации
    public void PlayParticles()
    {
        if (effectParticles != null)
        {
            StartCoroutine(PlayWithDelay());
        }
    }

    private IEnumerator PlayWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeParticles);
        effectParticles.Play();
        Debug.Log("[SecondObjectController] Particles played");
    }
}
