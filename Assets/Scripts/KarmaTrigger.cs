using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class KarmaTrigger : MonoBehaviour
    {
        public UnityEvent OnGoodKarma;
        public UnityEvent OnBadKarma;

        public bool debug;
        public bool goodEnding;
        
        public void Trigger()
        {
            int altWorldScore = KarmaCounter.AltWorldScore;
            int goodWorldScore = KarmaCounter.RealWorldScore;

            if (debug)
            {
                if (goodEnding)
                {
                    OnGoodKarma.Invoke();
                }
                else
                {
                    OnBadKarma.Invoke();
                }
                return;
            }
            
            if (altWorldScore > goodWorldScore)
            {
                OnBadKarma.Invoke();
            }
            else
            {
                OnGoodKarma.Invoke();
            }
        }
    }
}