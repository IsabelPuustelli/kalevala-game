using UnityEngine;

public class AnimationParameters : StateMachineBehaviour
{
    [SerializeField] bool _rootMotion = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => animator.applyRootMotion = _rootMotion;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInteracting", false);
        animator.applyRootMotion = false;
    }
}
