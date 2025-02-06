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
        //  ���ο� ���·� ���� �� ����
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    ó���� ������ �������� ������ �� ������ ������ ����
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���°� ���� ���·� �ٲ�� ������ ����
        if(updateOnState)
        {
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    MonoBehaviour.OnAnimatorMove ���Ŀ� ����
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    MonoBehaviour.OnAnimatorIK ���Ŀ� ����
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ���� 
        // ���� ���� ĳ���Ͱ� � ���¿� ���� �� � �ִϸ��̼��� �����ϰ�, � �ൿ�� ������ ���� �̸� ���ص� ��Ģ�� ���� ó���� ����
        // �ִϸ����Ϳ� �ִ� ȭ��ǥ�� ����
        // �� ȭ��ǥ�� �̾����ִ� �ִϸ��̼ǵ鳢�� ��ü �� Ÿ�̹��� ���� �� �����̶�� �Ҹ���
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
        if (updateOnStateMachine)
            animator.SetBool(boolName, valueOnExit);
    }
}
