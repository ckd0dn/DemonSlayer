using UnityEngine;

public class PlayerRollState : PlayerGroundState
{
    Vector2 tmpDir;
    public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //tmpDir = stateMachine.Player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        //stateMachine.Player.Rigidbody.AddForce((tmpDir) * groundData.DodgeSpeedModifier, ForceMode2D.Impulse);
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.RollParameterHash);
        stateMachine.Player.gameObject.layer = 25;
        stateMachine.IsRoll = true;
        SoundManager.Instance.PlaySFX(stateMachine.Player.rollClip); 
    }
    
    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RollParameterHash);
        stateMachine.Player.gameObject.layer = 3;
        stateMachine.IsRoll = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (stateMachine.Player.IsAnimationFinished())
        {            
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}