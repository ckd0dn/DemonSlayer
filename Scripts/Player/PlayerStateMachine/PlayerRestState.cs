using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRestState : PlayerGroundState
{
    public PlayerRestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FullRestParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FullRestParameterHash);
    }

    public override void Update()
    {
        if(stateMachine.Player.IsAnimationFinished())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
}
