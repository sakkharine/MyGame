using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
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
        _button = GetComponent<Button>();
        _image.sprite = first;
        
        _button.onClick.AddListener(Swap);
    }
    
    private void Swap()
    {
        isFirst = !isFirst;
        _image.sprite = isFirst ? first : second;
    }
}
