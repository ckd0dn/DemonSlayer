
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public float JumpForce { get; set; }

    public bool IsAttacking { get; set; }
    public bool IsRoll { get; set; } = false;
    public bool IsSkillActive { get; set; } = false;
    public bool IsDoubleJumped { get; set; } = false;
    public int ComboIndex { get; set; }
    

    public Transform MainManTransform { get; set; }

    public PlayerHurtState HurtState { get;}
    public PlayerDeathState DeathState { get; }
    public PlayerDashState DashState { get; }
    public PlayerRestState RestState { get; }
    public PlayerIdleState IdleState { get; }
    public PlayerRunState RunState { get; }
    public PlayerRollState RollState { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerDoubleJumpState DoubleJumpState { get; }
    public PlayerFallState FallState { get; }
    public PlayerAirAttackState AirAttackState { get; }
    public PlayerComboAttackState ComboAttackState { get; }
    public PlayerSkillState SkillState { get; }
    public PlayerFirstSkillState FirstSkillState { get; }
    public PlayerSecondSkillState SecondSkillState { get; }
    public PlayerThirdSkillState ThirdSkillState { get; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        MainManTransform = Camera.main.transform;

        HurtState = new PlayerHurtState(this);
        DeathState = new PlayerDeathState(this);
        DashState = new PlayerDashState(this);
        RestState = new PlayerRestState(this);

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        RollState = new PlayerRollState(this);

        JumpState = new PlayerJumpState(this);
        DoubleJumpState = new PlayerDoubleJumpState(this);
        FallState = new PlayerFallState(this);
        AirAttackState = new PlayerAirAttackState(this);

        ComboAttackState = new PlayerComboAttackState(this);

        SkillState = new PlayerSkillState(this);
        FirstSkillState = new PlayerFirstSkillState(this);
        SecondSkillState = new PlayerSecondSkillState(this);
        ThirdSkillState = new PlayerThirdSkillState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
    }

    public bool ReturnCurrentState(IState state)
    {
        return currentState == state;       
    }
}

