using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ButtonHint : MonoBehaviour
    {
        private static readonly HashSet<string> _shownHints = new();

        [SerializeField] private Button button;

        private void Awake()
        {
            if (_shownHints.Contains(gameObject.name))
            {
                Destroy(gameObject);
                return;
            }

            button.onClick.AddListener(Destroy);
        }
        
        private void Destroy()
        {
            _shownHints.Add(gameObject.name);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(Destroy);
        }
    }
}