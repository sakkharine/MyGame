using UnityEngine;

public class SwitchHumanAndBird : MonoBehaviour
{
    public Animator animator;
    public characterController characterControllerObject;
    public Flying flyingObject;

    private bool isHuman = true;

    private void Start()
    {
        MobileInput.Instance.CanSwitchForm = true;
    }

    private void Update()
    {
        if (!MobileInput.Instance.SwitchFormDown && !Input.GetKeyDown(KeyCode.G)) return;

        isHuman = !isHuman;
        if (isHuman)
        {
            animator.SetTrigger("Human");
            characterControllerObject.enabled = true;
            flyingObject.enabled = false;
        }
        else
        {
            Prev();
        }
    }

    public void Prev()
    {
        animator.SetTrigger("Prev");
        characterControllerObject.enabled = false;
        flyingObject.enabled = true;
    }
}
