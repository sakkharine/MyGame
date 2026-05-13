using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageByBool : MonoBehaviour
{
    [SerializeField] private bool value;
    [SerializeField] private Sprite whenTrue;
    [SerializeField] private Sprite whenFalse;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = value ? whenTrue : whenFalse;
    }
}
