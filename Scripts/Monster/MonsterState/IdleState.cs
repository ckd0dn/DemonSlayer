using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState : MonsterBaseState
{
    public bool isStopping = false;
    protected float stopDuration = 2f;
    protected float stopStartTime = 0f;

    protected float directionChangeMinTime = 3f;
    protected float directionChangeMaxTime = 6f;
    protected float directionChangeTimer = 0f;

    protected float minHeight = 1.3f;
    protected float maxHeight = 1.7f;

    public IdleState(MonsterStateMachine stateMachine) : base(stateMachine)
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
            if (!isStopping)
            {
                ChangeDirection();
                stateMachine.Monster.transform.Translate(Vector2.right * stateMachine.Monster.stats.speed * Time.deltaTime);
            }
            int randomStop = Random.Range(0, 5000);

            if (randomStop > 4998 && !isStopping)
            {
                isStopping = true;
            }

            if (isStopping)
            {
                MonsterStop();
            }

            AttackByRange();
        }
    }

    public void ChangeDirection()
    {
        directionChangeMaxTime = Random.Range(directionChangeMinTime, directionChangeMaxTime);
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeMaxTime)
        {
            directionChangeTimer = 0f;
            float randomValue = Random.value;
            bool isRight = randomValue < 0.5f;
            stateMachine.Monster.isRight = isRight;
            stateMachine.Monster.transform.eulerAngles = isRight ? new Vector3(0, 180, 0) : Vector3.zero;
        }
    }

    public void MonsterStop()
    {
        stopStartTime += Time.deltaTime;

        if (stopStartTime >= stopDuration)
        {
            isStopping = false;
            stopStartTime = 0f;
        }
    }


    public override void Exit()
    {
        stateMachine.Monster.anim.SetBool(AnimationHashes.Idle, false);
    }

    public void AttackByRange()
    {
        float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);
        float CanChasingheight = stateMachine.Monster.player.transform.position.y - stateMachine.Monster.transform.position.y;
        if (CanChasingheight > minHeight && CanChasingheight < maxHeight)
        {
            if (stateMachine.Monster.canstats.attackDetectRange != null)
            {
                for (int i = 0; i < stateMachine.Monster.canstats.attackDetectRange.Length; i++)
                {
                    if (distance <= stateMachine.Monster.canstats.attackDetectRange[i])
                    {
                        stateMachine.Monster.attackPatternType = (AttackPatternType)i;
                        stateMachine.ChangeState(stateMachine.attackingState);
                        return;
                    }
                }

                if (distance < stateMachine.Monster.canstats.detectRange)
                {
                    stateMachine.ChangeState(stateMachine.chasingState);
                    return;
                }
            }
        }
    }

}
