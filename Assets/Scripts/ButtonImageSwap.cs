using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonImageSwap : MonoBehaviour
{
    [SerializeField] private Sprite first;
    [SerializeField] private Sprite second;

    private Image _image;
    private Button _button;

    private bool isFirst = true;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = first;
    }
    
    public void Swap()
    {
        isFirst = !isFirst;
        _image.sprite = isFirst ? first : second;
    }
}
