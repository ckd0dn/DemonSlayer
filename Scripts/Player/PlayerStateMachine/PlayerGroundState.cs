using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        stateMachine.Player.isJumped = false;
        stateMachine.IsDoubleJumped = false;
        stateMachine.Player.PlayerSkill.isDashing = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!stateMachine.Player.isGrounded && stateMachine.Player.Rigidbody.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }
    
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (!stateMachine.IsRoll && !stateMachine.Player.isSkillActive)
        {
            if (stateMachine.MovementSpeedModifier == 0)
            {
                stateMachine.MovementSpeedModifier = 1f;
            }
            base.OnJumpStarted(context);
            stateMachine.ChangeState(stateMachine.JumpState);
        }
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }

    protected override void OnDodgeStarted(InputAction.CallbackContext context)
    {
        base.OnDodgeStarted(context);
        if (stateMachine.Player.DashGet)
        {
            stateMachine.ChangeState(stateMachine.DashState);
        }
        else if(!stateMachine.Player.DashGet && !stateMachine.IsRoll)
        stateMachine.ChangeState(stateMachine.RollState);
    }

    protected override void OnFirstSkillStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.firstSkillSlot == true)
        {
            if (stateMachine.Player.playerEquipSkill[0] == null) return;
        }
        else
        {
            if (stateMachine.Player.playerEquipSkill[3] == null) return;
        }
        base.OnFirstSkillStarted(context);
        stateMachine.ChangeState(stateMachine.FirstSkillState);
    }

    protected override void OnSecondSkillStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.firstSkillSlot == true)
        {
            if (stateMachine.Player.playerEquipSkill[1] == null) return;
        }
        else
        {
            if (stateMachine.Player.playerEquipSkill[4] == null) return;
        }
        base.OnSecondSkillStarted(context);
        stateMachine.ChangeState(stateMachine.SecondSkillState);
    }

    protected override void OnThirdSkillStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.Player.firstSkillSlot == true)
        {
            if (stateMachine.Player.playerEquipSkill[2] == null) return;
        }
        else
        {
            if (stateMachine.Player.playerEquipSkill[5] == null) return;
        }
        base.OnThirdSkillStarted(context);
        stateMachine.ChangeState(stateMachine.ThirdSkillState);
    }
}