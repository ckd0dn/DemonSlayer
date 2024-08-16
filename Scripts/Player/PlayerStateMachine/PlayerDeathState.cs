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
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        stateMachine.Player.gameObject.layer = 25;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.DeathParameterHash);
        GameManager.Instance.Player.healthSystem.isInvincibility = true;
    }

    public override void Exit() 
    {
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints2D.None;
        stateMachine.Player.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        stateMachine.Player.gameObject.layer = 3;
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DeathParameterHash);
        GameManager.Instance.Player.healthSystem.isInvincibility = false;
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
