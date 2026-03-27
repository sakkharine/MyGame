using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace QuestGame
{
    public class Timer : MonoBehaviour
    {
        public float duration;
        
        public bool StartOnAwake;
        public UnityEvent OnEnd;    
        public UnityEvent<float> OnStart;

        private bool isRunning;
        private Coroutine timerCoroutine;
        
        private void Start()
        {
            if (StartOnAwake)
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            if (isRunning) return;
            isRunning = true;
            OnStart.Invoke(duration);
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        public void Cancel()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
        }
        
        private IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(duration);
            OnEnd.Invoke();
            isRunning = false;
            
            timerCoroutine = null;
        }
    }
}