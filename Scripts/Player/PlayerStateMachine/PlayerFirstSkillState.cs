using System;
using UnityEngine;

public class PlayerFirstSkillState : PlayerSkillState
{
    public int SkillHash { get; set;}
    int index = 99;
    public string skillAniName;

    public PlayerFirstSkillState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("start1Skill");
        stateMachine.Player.isSkillActive = true;
        base.Enter();
        if(stateMachine.Player.firstSkillSlot == true)
        {
            index = 0;
        }
        else
        {
            index = 3;
        }
        if (index == 99) return;
        if(stateMachine.Player.healthSystem.CurrentMana < stateMachine.Player.playerEquipSkill[index].MPCost)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        SkillAnimationSelect(index);     
    }

    public override void Exit() 
    {
        stateMachine.Player.isSkillActive = false;
        base.Exit();
        if(index == 99) return;
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
        if (stateMachine.Player.healthSystem.CurrentMana < stateMachine.Player.playerEquipSkill[index].MPCost) return;
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StartAnimation(SkillHash);
    }

    public void StopSkillState(int index)
    {
        string skillString = stateMachine.Player.playerEquipSkill[index].AnimationName;
        SkillHash = Animator.StringToHash(skillString);
        StopTriggerAnimation(SkillHash);
    }
}