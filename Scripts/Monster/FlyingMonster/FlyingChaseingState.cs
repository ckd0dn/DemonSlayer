using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingChaseingState : ChasingState
{
    public FlyingChaseingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Update()
    {
        //플레이어 바라보기
        IsLookPlayer(stateMachine.Monster.transform);

        float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);
        Vector3 playerPosition = stateMachine.Monster.player.transform.position;

        // 몬스터 이동
        stateMachine.Monster.transform.position = Vector3.MoveTowards(stateMachine.Monster.transform.position, playerPosition, stateMachine.Monster.stats.speed * stateMachine.Monster.stats.chasingspeed * Time.deltaTime);


        if (CanStartAttack(stateMachine.Monster))
        {
            // 공격 범위 내에 있을 때
            AttackByRange();
        }
        // 플레이어가 범위를 벗어났을 때
        else if (distance > stateMachine.Monster.canstats.detectRange)
        {
            if (stateMachine.Monster.dir.x > 0)
            {
                stateMachine.Monster.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                stateMachine.Monster.isRight = false;
            }
            else
            {
                stateMachine.Monster.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                stateMachine.Monster.isRight = true;
            }
            stateMachine.ChangeState(stateMachine.idleState);

        }
    }
}
