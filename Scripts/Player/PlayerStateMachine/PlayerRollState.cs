using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollState : PlayerGroundState
{
    Vector2 tmpDir;
    public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.preventFlipX = false;
        stateMachine.Player.PlayerSkill.RollStart();
        stateMachine.Player.healthSystem.isInvincibility = true;
        StartAnimation(stateMachine.Player.AnimationData.RollParameterHash);
        stateMachine.Player.gameObject.layer = 25;
        stateMachine.IsRoll = true;
    }
    
    public override void Exit() 
    { 
        base.Exit();
        stateMachine.Player.healthSystem.isInvincibility = false;
        StopAnimation(stateMachine.Player.AnimationData.RollParameterHash);
        stateMachine.Player.gameObject.layer = 3;
        stateMachine.IsRoll = false;
    }

    public override void Update()
    {       
        if (stateMachine.Player.IsAnimationFinishedWithName("Roll"))
        {            
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
}