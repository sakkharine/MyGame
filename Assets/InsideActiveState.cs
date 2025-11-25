using UnityEngine;

public class InsideActiveState : StateMachineBehaviour
{
    [Tooltip("Имя булевой переменной, которую нужно переключать")]
    public string flagName = "insideActive";

    // Вызывается, когда анимация входит в состояние
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(flagName, true);
        Debug.Log($"[Animator] {flagName} = true (вход в {stateInfo.shortNameHash})");
    }

}


