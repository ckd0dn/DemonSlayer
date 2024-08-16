using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    float dashStartAnimeDelay = 0f;
    bool dashStart = false;
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //if (stateMachine.Player.PlayerSkill.isDashing) stateMachine.ChangeState(stateMachine.IdleState);
        dashStart = true;
        stateMachine.Player.healthSystem.isInvincibility = true;
        stateMachine.Player.PlayerSkill.DashStart();
        StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        stateMachine.Player.gameObject.layer = 25;
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        stateMachine.Player.gameObject.layer = 3;
        stateMachine.Player.healthSystem.isInvincibility = false;
        dashStart = false;
    }

    public override void Update()
    {
        //if(dashStart) DashTimeCheck();
        if (stateMachine.Player.IsAnimationFinishedWithName("Dash"))
        {
            if (stateMachine.Player.Rigidbody.velocity.y <= 0)
            {
                stateMachine.ChangeState(stateMachine.FallState);
                return;
            }
            else if (stateMachine.Player.isGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
        //if (dashStartAnimeDelay > 1f)
        //{
        //    if (stateMachine.Player.Rigidbody.velocity.y <= 0)
        //    {
        //        stateMachine.ChangeState(stateMachine.FallState);
        //        dashStartAnimeDelay = 0f;
        //        dashStart = false;
        //        return;
        //    }
        //    else if (stateMachine.Player.isGrounded)
        //    {
        //        stateMachine.ChangeState(stateMachine.IdleState);
        //        dashStartAnimeDelay = 0f;
        //        dashStart = false;
        //        return;
        //    }
        //}
        //else
        //{
        //    DashTimeCheck();
        //}
    }

    public void DashTimeCheck()
    {
        dashStartAnimeDelay += Time.deltaTime;
    }
}
