using System;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { get; private set; }

    [Header("Movement")]
    [SerializeField] private HoldButton leftButton;
    [SerializeField] private HoldButton rightButton;

    [Header("Actions")]
    [SerializeField] private HoldButton interactButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private HoldButton jumpHold;
    [SerializeField] private Button switchWorldButton;
    [SerializeField] private Button switchFormButton;

    [Header("Images")]
    [SerializeField] private ButtonImageSwap birdSwapImage;
    
    public float Horizontal
    {
        get
        {
            if(leftButton.IsHeld) return -1f;
            if(rightButton.IsHeld) return 1f;
            return 0f;
        }
    }
    
    public bool IsInteracting => interactButton.IsActive() && interactButton.IsHeld;

    public bool CanInteract
    {
        set => interactButton.gameObject.SetActive(value);
    }
    
    public bool CanSwitchWorld
    {
        set => switchWorldButton.gameObject.SetActive(value);
    }
    
    public bool CanSwitchForm
    {
        set => switchFormButton.gameObject.SetActive(value);
    }
    
    public bool JumpDown { get; private set; }
    public bool JumpHold => jumpHold.IsHeld;
    public bool SwitchWorldDown { get; private set; }
    public bool SwitchFormDown { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        
        jumpButton.onClick.AddListener(() => JumpDown = true);
        switchWorldButton.onClick.AddListener(() => SwitchWorldDown = true);
        switchFormButton.onClick.AddListener(() => SwitchFormDown = true);
    }

    private void LateUpdate()
    {
        JumpDown = false;
        SwitchWorldDown = false;
        SwitchFormDown = false;
    }

    public void SwapBirdImage()
    {
        birdSwapImage.Swap();
    }
}
