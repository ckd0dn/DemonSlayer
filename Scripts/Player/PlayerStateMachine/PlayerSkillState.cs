using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerGroundState
{

    public PlayerSkillState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.SkillParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SkillParameterHash);
    }
}
