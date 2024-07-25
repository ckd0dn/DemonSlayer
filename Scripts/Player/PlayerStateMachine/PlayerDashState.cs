using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player.PlayerSkill.DashStart();
        //Vector2 tmpDir = stateMachine.Player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        //Debug.Log($"flipX: {stateMachine.Player.spriteRenderer.flipX}");
        //Debug.Log($"tmpDir: {tmpDir}");
        //Debug.Log($"DashForce: {groundData.DashForce}");
        //Debug.Log($"Rigidbody position: {stateMachine.Player.Rigidbody.position}");
        //stateMachine.Player.Rigidbody.AddForce((tmpDir) * groundData.DashForce, ForceMode2D.Impulse);
        //stateMachine.Player.Rigidbody.velocity = new Vector2(tmpDir.x * groundData.DashForce, stateMachine.Player.Rigidbody.velocity.y);
        base.Enter();
        StartTriggerAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        stateMachine.Player.gameObject.layer = 25;

    }

    public override void Exit() 
    { 
        //stateMachine.Player.PlayerSkill.DashStop();
        //stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Exit();
        StopTriggerAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        stateMachine.Player.gameObject.layer = 3;

    }

    public override void Update()
    {
      
        if(stateMachine.Player.IsAnimationFinished())
        {
            if (stateMachine.Player.Rigidbody.velocity.y <= 0)
            {
                stateMachine.ChangeState(stateMachine.FallState);
                return;
            }
            else if(stateMachine.Player.isGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
    }
}
