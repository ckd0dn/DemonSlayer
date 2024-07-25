using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.DeathParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DeathParameterHash);
    }

    public override void Update()
    {
        if(stateMachine.Player.isRespawn == true)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            stateMachine.Player.isRespawn = false;
        }
    }

}
