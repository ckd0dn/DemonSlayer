public class PlayerAirAttackState : PlayerAirState
{
    private bool alreadyAppliedCombo;
    private bool alreadyApplyForce;

    AirAttackInfoData AirattackInfo;
    public PlayerAirAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartTriggerAnimation(stateMachine.Player.AnimationData.AirAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopTriggerAnimation(stateMachine.Player.AnimationData.AirAttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.isGrounded)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
