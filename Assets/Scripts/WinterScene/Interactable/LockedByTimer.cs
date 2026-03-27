using UnityEngine;

namespace QuestGame
{
    public class LockedByTimer : MonoBehaviour, ILock
    {
        private float startTime;
        public float duration;

        public bool IsLocked { get; private set; }

        private void Start()
        {
            startTime = Time.time;
            IsLocked = true;
        }

        private void Update()
        {
            if (Time.time - startTime >= duration)
            {
                IsLocked = false;
            }
        }
    }
}