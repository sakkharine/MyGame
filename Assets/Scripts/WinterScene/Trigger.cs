using System;
using UnityEngine;
using UnityEngine.Events;

namespace QuestGame
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : MonoBehaviour
    {
        public bool DisableAfterEnter = false;
        
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnEnter.Invoke();            
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit.Invoke();
        }

        private void OnValidate()
        {
            var collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }
}