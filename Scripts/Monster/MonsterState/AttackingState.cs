using UnityEngine;



public class AttackingState : MonsterBaseState
{
    public AttackingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        CheckAttactPattern(stateMachine.Monster.attackPatternType);
        IsLookPlayer(stateMachine.Monster.transform);
    }

    public override void Update()
    {
        var animatorStateInfo = stateMachine.Monster.anim.GetCurrentAnimatorStateInfo(0);

        float normalizedtime = animatorStateInfo.normalizedTime % 1;

        if (normalizedtime >= stateMachine.Monster.animationEndTime)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    public override void Exit()
    {
        CheckAndOffAnimation(AnimationHashes.Attack1);
        CheckAndOffAnimation(AnimationHashes.Attack2);
        CheckAndOffAnimation(AnimationHashes.Attack3);
        stateMachine.Monster.StartAttackDelay();
    }

    public void CheckAttactPattern(AttackPatternType type)
    {
        switch (type)
        {
            case AttackPatternType.First:
                CheckAndOnAnimation(AnimationHashes.Attack1);
                break;
            case AttackPatternType.Second:
                CheckAndOnAnimation(AnimationHashes.Attack2);
                break;
            case AttackPatternType.Third:
                CheckAndOnAnimation(AnimationHashes.Attack3);
                break;
            default:
                break;
        }
    }

    private void CheckAndOnAnimation(int hash)
    {
        if (HasParameter(hash))
        {
            stateMachine.Monster.anim.SetBool(hash, true);
        }
    }

    private void CheckAndOffAnimation(int hash)
    {
        if (HasParameter(hash))
        {
            stateMachine.Monster.anim.SetBool(hash, false);
        }
    }

    // 주어진 해시가 애니메이터 파라미터에 존재하는지 확인
    private bool HasParameter(int hash)
    {
        foreach (AnimatorControllerParameter param in stateMachine.Monster.anim.parameters)
        {
            if (param.nameHash == hash)
            {
                return true;
            }
        }
        return false;
    }
}




//if (animatorStateInfo.shortNameHash == AnimationHashes.Attack1)
//{
//    if (normalizedtime > 0.5f && normalizedtime < 0.6f)
//    {
//        Attack(stateMachine.Monster.firstAttackType);
//    }

//}
//else if(animatorStateInfo.shortNameHash == AnimationHashes.Attack2)
//{
//    if (normalizedtime > 0.5f && normalizedtime < 0.6f)
//    {
//        Attack(stateMachine.Monster.secondAttackType);
//    }

//}
//else if (animatorStateInfo.shortNameHash == AnimationHashes.Attack3)
//{
//    if (normalizedtime > 0.5f && normalizedtime < 0.6f)
//    {
//        Attack(stateMachine.Monster.secondAttackType);
//    }

//}


//public void Attack(AttackType attackType)
//{
//    switch (attackType)
//    {
//        case AttackType.MeleeAttack:
//            stateMachine.Monster.monsterAttack.MeleeAttack();
//            break;
//        case AttackType.DestructAttack:
//            stateMachine.Monster.monsterAttack.DestructAttack();
//            break;
//        case AttackType.RushAttack:
//            stateMachine.Monster.monsterAttack.RushAttack();
//            break;
//        //case AttackType.StraightRangedAttack:
//        //    stateMachine.Monster.monsterAttack.StraightRangedAttack();
//        //    break;
//        //case AttackType.ParabolaRangedAttack:
//        //    stateMachine.Monster.monsterAttack.ParabolaRangedAttack();
//        //    break;
//        default:
//            break;
//    }
//}