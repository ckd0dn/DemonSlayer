using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerBaseState
{
    public PlayerHurtState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if(stateMachine.ReturnCurrentState(stateMachine.SkillState)) return;
        //stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartTriggerAnimation(stateMachine.Player.AnimationData.HurtParameterHash);       
    }

    public override void Exit() 
    {
        base.Exit();
        StopTriggerAnimation(stateMachine.Player.AnimationData.HurtParameterHash);
    }

    public override void Update()
    {
        if (stateMachine.Player.IsAnimationFinished())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }        
    }
}
