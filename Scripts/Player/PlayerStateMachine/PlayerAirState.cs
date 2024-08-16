using UnityEngine.InputSystem;

public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AirParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.IsAttacking)
        {
            OnAirAttack();
            return;
        }
    }

    protected virtual void OnAirAttack()
    {
        if(!stateMachine.ReturnCurrentState(stateMachine.AirAttackState))
        {
            stateMachine.ChangeState(stateMachine.AirAttackState);
        }
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        if (stateMachine.IsDoubleJumped) return;
        //if (!stateMachine.Player.DoubleJumpGet) return;

        stateMachine.ChangeState(stateMachine.DoubleJumpState);
        stateMachine.IsDoubleJumped = true;
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
        if (stateMachine.Player.DashGet)
        {
            if(!stateMachine.Player.PlayerSkill.isDashing)
            stateMachine.ChangeState(stateMachine.DashState);
        }
    }
}