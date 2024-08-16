using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChaseingState : ChasingState
{
    public BombChaseingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Update()
    {
        //플레이어 바라보기
        IsLookPlayer(stateMachine.Monster.transform);

        float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);
        Vector3 playerPosition = stateMachine.Monster.player.transform.position;
        playerPosition.y = stateMachine.Monster.transform.position.y;

        // 몬스터 이동
        stateMachine.Monster.transform.position = Vector3.MoveTowards(stateMachine.Monster.transform.position, playerPosition, stateMachine.Monster.stats.chasingspeed * Time.deltaTime);

        var animatorStateInfo = stateMachine.Monster.anim.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.shortNameHash == AnimationHashes.Chase)
        {
            float normalizedtime = animatorStateInfo.normalizedTime % 1;
            if (normalizedtime >= 0.9f)
            {
                stateMachine.ChangeState(stateMachine.attackingState);
            }
        }
    }

}
