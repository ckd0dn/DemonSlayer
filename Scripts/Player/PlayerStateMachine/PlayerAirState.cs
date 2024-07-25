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
        if (UIManager.Instance.onUI) return;
        base.OnJumpStarted(context);
        if (stateMachine.IsDoubleJumped) return;
        //if (!stateMachine.Player.DoubleJumpGet) return;

        stateMachine.ChangeState(stateMachine.DoubleJumpState);
        stateMachine.IsDoubleJumped = true;
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.onUI) return;
        base.OnDodgeStarted(context);
        if (stateMachine.Player.DashGet)
        {
            stateMachine.ChangeState(stateMachine.DashState);
        }
    }
}