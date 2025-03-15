using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    private EntityStateMachine _stateMachine => GetComponentInParent<EntityView>().Controller.StateMachine;

    public void OnAnimationEnd()
    {
        _stateMachine.OnAnimationEnd();
    }





}
