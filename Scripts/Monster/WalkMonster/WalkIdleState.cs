using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkIdleState : IdleState
{
    public WalkIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.Monster.anim.SetBool(AnimationHashes.Idle, true);
        
    }

    public override void Update()
    {
        if (!stateMachine.Monster.isAttacking)
        {
            int randomStop = Random.Range(0, 5000);

            if (!isStopping)
            {
                stateMachine.Monster.anim.SetBool(AnimationHashes.Walk, true);
                stateMachine.Monster.anim.SetBool(AnimationHashes.Idle, false);
                ChangeDirection();
                stateMachine.Monster.transform.Translate(Vector2.right * stateMachine.Monster.stats.speed * Time.deltaTime);
            }

            if (randomStop > 4998 && !isStopping)
            {
                isStopping = true;
            }

            if (isStopping)
            {
                stateMachine.Monster.anim.SetBool(AnimationHashes.Walk, false);
                stateMachine.Monster.anim.SetBool(AnimationHashes.Idle, true);
                stopStartTime += Time.deltaTime;

                if (stopStartTime >= stopDuration)
                {
                    isStopping = false;
                    stopStartTime = 0f;
                }
            }
            AttackByRange();
        }
    }
    public override void Exit()
    {
        stateMachine.Monster.anim.SetBool(AnimationHashes.Idle, false);
        stateMachine.Monster.anim.SetBool(AnimationHashes.Walk, false);
    }
}
