using JetBrains.Annotations;
using System;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string hurtParameterName = "Hurt";
    [SerializeField] private string deathParameterName = "Death";
    [SerializeField] private string dashParameterName = "Dash";
    [SerializeField] private string restParameterName = "Rest";
    [SerializeField] private string toRestParameterName = "ToRest";
    [SerializeField] private string outRestParameterName = "OutRest";
    [SerializeField] private string fullRestParameterName = "FullRest";

    [SerializeField] private string groundParameter = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string rollParameterName = "Roll";

    [SerializeField] private string airParameterName = "@Air";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string doubleJumpParameterName = "DoubleJump";
    [SerializeField] private string doubleJumpEffectParameterName = "DoubleJumpEffect";
    [SerializeField] private string fallParameterName = "Fall";
    [SerializeField] private string airAttackParameterName = "AirAttack";

    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string comboAttackParameterName = "ComboAttack";

    [SerializeField] private string skillParameterName = "@Skill";
    [SerializeField] private string holySlashParameterName = "HolySlash";
    [SerializeField] private string lightCutParameterName = "LightCut";
    [SerializeField] private string holyHealParameterName = "HolyHeal";
    [SerializeField] private string saintHealParameterName = "SaintHeal";
    [SerializeField] private string swordBuffParameterName = "SwordBuff";
    [SerializeField] private string shieldBuffParameterName = "ShieldBuff";

    public int HurtParameterHash {  get; private set; }
    public int DeathParameterHash { get; private set; }
    public int DashParameterHash {  get; private set; }
    public int RestParameterHash {  get; private set; } 
    public int ToRestParameterHash { get; private set; }
    public int OutRestParameterHash { get; private set; }
    public int FullRestParameterHash { get; private set; }
    public int GroundParameterHash {  get; private set; }
    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int AirParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int DoubleJumpParameterHash { get; private set; }
    public int DoubleJumpEffectParameterHash { get; private set; }
    public int FallParameterHash { get; private set; }
    public int AirAttackParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int ComboAttackParameterHash { get; private set; }
    public int SkillParameterHash { get; private set; }
    public int HolySlashParameterHash { get; private set; }
    public int LightCutParameterHash { get; private set; }
    public int HolyHealParameterHash { get; private set; }
    public int SaintHealParameterHash { get; private set; }
    public int SwordBuffParameterHash { get; private set; }
    public int ShieldBuffParameterHash { get; private set; }

    public void Initialize()
    {
        HurtParameterHash = Animator.StringToHash(hurtParameterName);
        DeathParameterHash = Animator.StringToHash(deathParameterName);
        DashParameterHash = Animator.StringToHash(dashParameterName);
        RestParameterHash = Animator.StringToHash(restParameterName);
        ToRestParameterHash = Animator.StringToHash(toRestParameterName);
        OutRestParameterHash = Animator.StringToHash(outRestParameterName);
        FullRestParameterHash = Animator.StringToHash(fullRestParameterName);

        GroundParameterHash = Animator.StringToHash(groundParameter);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);

        AirParameterHash = Animator.StringToHash(airParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        DoubleJumpParameterHash = Animator.StringToHash(doubleJumpParameterName);
        DoubleJumpEffectParameterHash = Animator.StringToHash(doubleJumpEffectParameterName);
        FallParameterHash = Animator.StringToHash(fallParameterName);
        AirAttackParameterHash = Animator.StringToHash(airAttackParameterName);

        AttackParameterHash = Animator.StringToHash(attackParameterName);
        ComboAttackParameterHash = Animator.StringToHash(comboAttackParameterName);

        SkillParameterHash = Animator.StringToHash(skillParameterName);
        HolySlashParameterHash = Animator.StringToHash(holySlashParameterName);
        LightCutParameterHash = Animator.StringToHash(lightCutParameterName);
        HolyHealParameterHash = Animator.StringToHash(holyHealParameterName);
        SaintHealParameterHash = Animator.StringToHash(saintHealParameterName);
        SwordBuffParameterHash = Animator.StringToHash(swordBuffParameterName);
        ShieldBuffParameterHash = Animator.StringToHash(shieldBuffParameterName);
    }
}