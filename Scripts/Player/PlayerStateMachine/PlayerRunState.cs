using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
    }

    protected override void OnFirstSkillStarted(InputAction.CallbackContext context)
    {
        base.OnFirstSkillStarted(context);
    }

    protected override void OnSecondSkillStarted(InputAction.CallbackContext context)
    {
        base.OnSecondSkillStarted(context);
    }

    protected override void OnThirdSkillStarted(InputAction.CallbackContext context)
    {
        base.OnThirdSkillStarted(context);
    }
}