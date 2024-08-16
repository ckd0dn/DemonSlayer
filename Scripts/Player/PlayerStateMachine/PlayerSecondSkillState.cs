using System;
using UnityEngine;

public class PlayerSecondSkillState : PlayerSkillState
{
    public PlayerSecondSkillState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    int index = 99;
    public int SkillHash { get; set; }

    public override void Enter()
    {
        stateMachine.Player.isSkillActive = true;
        base.Enter();
        if (stateMachine.Player.firstSkillSlot == true)
        {
            index = 1;
        }
        else
        {
            index = 4;
        }
        if (index == 99) return;
        if (stateMachine.Player.healthSystem.CurrentMana < stateMachine.Player.playerEquipSkill[index].MPCost)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        SkillAnimationSelect(index);
    }

    public override void Exit()
    {
        stateMachine.Player.isSkillActive = false;
        base.Exit();
        if (index == 99) return;
        StopSkillState(index);
    }

    public override void Update()
    {
        if (stateMachine.Player.IsAnimationFinishedWithName(stateMachine.Player.playerEquipSkill[index].AnimationName))
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public void SkillAnimationSelect(int index)
    {
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StartTriggerAnimation(SkillHash);
    }

    public void StopSkillState(int index)
    {
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StopTriggerAnimation(SkillHash);
    }
}
