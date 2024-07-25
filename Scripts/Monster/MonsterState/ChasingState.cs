using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChasingState : IMonsterState
{
    public IEnumerator Execute(MonsterController monster)
    {
        while (monster.currentState == MonsterState.Chasing)
        {
            monster.IsLookPlayer(monster.transform);
            float distance = Vector3.Distance(monster.transform.position, monster.player.transform.position);
            Vector3 playerPosition = monster.player.transform.position;
            playerPosition.y = monster.transform.position.y;

            // 몬스터 이동
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, playerPosition, monster.stats.speed * Time.deltaTime);

            // 공격 범위 내에 있을 때
            if (distance <= monster.canstats.attackRange)
            {
                if (CanStartAttack(monster))
                {
                    monster.ChangeState(MonsterState.Attack);
                    yield break;
                }
            }
            // 플레이어가 범위를 벗어났을 때
            else if (distance > monster.canstats.range)
            {
                if(monster.dir.x > 0)
                {
                    monster.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    monster.isRight = false;
                }
                else
                {
                    monster.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    monster.isRight = true;
                }
                monster.ChangeState(MonsterState.Idle);
                yield break;
            }
            
            yield return null;
        }
        yield return null;
    }

    private bool CanStartAttack(MonsterController monster)
    {
        return HasState(monster.anim, AnimationHashes.Attack);
    }
    private bool HasState(Animator animator, int stateHash)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (animator.HasState(i, stateHash))
            {
                return true;
            }
        }
        return false;
    }
}


//플레이어와의 y축 차이가 얼마 이하일 때
//float playerAttackPos = monster.player.transform.position.y - monster.transform.position.y;

//if (playerAttackPos > 1.6f)
//{
//    monster.ChangeState(MonsterState.Idle);
//    yield break;
//}
//monster.MonsterChasingTrigger();