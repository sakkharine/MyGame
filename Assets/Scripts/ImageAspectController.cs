using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ImageAspectController : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;

        [ContextMenu("Update Aspect Ratio")]
        private void Start()
        {
            aspectRatioFitter.aspectRatio = image.sprite.rect.width / image.sprite.rect.height;
        }

        private void Update()
        {
            aspectRatioFitter.aspectRatio = image.sprite.rect.width / image.sprite.rect.height;
        }
    }
}