using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player.Rigidbody.velocity = Vector2.zero;
        stateMachine.Player.preventFlipX = true;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        stateMachine.Player.preventFlipX = false;
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        base.OnDodgeStarted(context);
        if (stateMachine.Player.DashGet)
        {
            stateMachine.ChangeState(stateMachine.DashState);
        }
        else if (!stateMachine.Player.DashGet)
            stateMachine.ChangeState(stateMachine.RollState);
    }
}
