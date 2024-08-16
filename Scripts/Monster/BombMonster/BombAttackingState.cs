using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttackingState : AttackingState
{
    
    public BombAttackingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Update()
    {
        var animatorStateInfo = stateMachine.Monster.anim.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.shortNameHash == AnimationHashes.Attack1)
        {
            float normalizedtime = animatorStateInfo.normalizedTime % 1;
            if (normalizedtime >= stateMachine.Monster.animationEndTime)
            {
                var destructMonster = stateMachine.Monster as BombMonster;
                if (destructMonster != null)
                {
                    destructMonster.OnBombDeathHandler();
                }
            }
        }
   
    }

}
