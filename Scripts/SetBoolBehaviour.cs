using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  새로운 상태로 변할 때 실행
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 상태가 다음 상태로 바뀌기 직전에 실행
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    MonoBehaviour.OnAnimatorMove 직후에 실행
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    MonoBehaviour.OnAnimatorIK 직후에 실행
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계로 전환이 왔을때 실행 
        // 상태 기계는 캐릭터가 어떤 상태에 있을 때 어떤 애니메이션을 실행하고, 어떤 행동을 취할지 등을 미리 정해둔 규칙에 따라 처리를 말함
        // 애니메이터에 있는 화살표를 말함
        // 즉 화살표로 이어져있는 애니메이션들끼리 교체 될 타이밍이 왔을 때 실행이라는 소리임
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계에서 빠져나올때 실행
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnExit);
    }
}
