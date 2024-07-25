using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IdleState :IMonsterState
{
    private bool isStopping = false;
    
    public IEnumerator Execute(MonsterController monster)
    {
        while (monster.currentState == MonsterState.Idle)
        {
            int randomStop = Random.Range(0, 5000);

            if (!isStopping)
            {
                monster.transform.Translate(Vector2.right * monster.stats.speed * Time.deltaTime);
            }
            if(Time.timeScale > 0.5f)
            {
                if (randomStop > 4998 && !isStopping)
                {
                    isStopping = true;
                    yield return new WaitForSeconds(2f); // 멈출 시간만큼 기다림
                    isStopping = false;
                }
            }
            //monster.MonsterChasingTrigger();

            float distance = Vector3.Distance(monster.transform.position, monster.player.transform.position);

            if (distance < monster.canstats.range)
            {
                monster.ChangeState(MonsterState.Chasing);
            }

            yield return null;
        }
        yield return null;
    }

}
