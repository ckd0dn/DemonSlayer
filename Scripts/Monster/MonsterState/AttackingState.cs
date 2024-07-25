using System.Collections;
using UnityEngine;

public class AttackingState : IMonsterState
{
    public IEnumerator Execute(MonsterController monster)
    {
        bool isAttacking= true;
        monster.IsLookPlayer(monster.transform);

        while (monster.currentState == MonsterState.Attack)
        {
            if (monster.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float normalizedTime = monster.anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
                if (normalizedTime > 0.5f && normalizedTime < 0.51f)
                {
                    monster.Attack();
                }

                if (normalizedTime >= 0.95f)
                {
                    isAttacking = false;
                }
            }

            float distance = Vector3.Distance(monster.transform.position, monster.player.transform.position);

            if (!isAttacking)
            {
                if (distance >= monster.canstats.attackRange)
                {
                    monster.ChangeState(MonsterState.Chasing);
                    yield break;
                }

                else if (distance >= monster.canstats.range)
                {
                    monster.ChangeState(MonsterState.Idle);
                    yield break;
                }
            }

            if(monster.currentState == MonsterState.Death)
            {
                monster.anim.StopPlayback();
            }
            yield return null;
        }
        yield return null;
    }

}