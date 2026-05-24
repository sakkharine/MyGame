using UnityEngine;

public class SwitchHumanAndBird : MonoBehaviour
{
    float humanStateCameraSize = 15.80588f;
    float birdStateCameraSize = 9;
    
    public Animator animator;
    public characterController characterControllerObject;
    public Flying flyingObject;
    public CameraFollowScript camera;
    
    private bool isHuman = true;

    private void Start()
    {
        MobileInput.Instance.CanSwitchForm = true;
    }

    private void Update()
    {
        if (!MobileInput.Instance.SwitchFormDown && !Input.GetKeyDown(KeyCode.G)) return;

        if (isHuman)
        {
            TurnToBird();
        }
        else
        {
            TurnToHuman();
        }
    }

    public void TurnToBird()
    {
        if(!isHuman)
            return;
        
        isHuman = false;
        MobileInput.Instance.SwapBirdImage();
        
        animator.SetTrigger("Prev");
        characterControllerObject.enabled = false;
        flyingObject.enabled = true;

        camera.Zoom(birdStateCameraSize, 1f);
    }
    
    public void TurnToHuman()
    {
        if(isHuman)
            return;

        isHuman = true;
        MobileInput.Instance.SwapBirdImage();
        
        animator.SetTrigger("Human");
        characterControllerObject.enabled = true;
        flyingObject.enabled = false;

        camera.Zoom(humanStateCameraSize, 1f);
    }
}
