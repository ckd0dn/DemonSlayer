using UnityEngine;

public class ChasingState : MonsterBaseState
{
    
    public ChasingState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Monster.anim.SetBool(AnimationHashes.Chase, true);
    }

    public override void Update()
    {
        //플레이어 바라보기
        IsLookPlayer(stateMachine.Monster.transform);

        float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);
        Vector3 playerPosition = stateMachine.Monster.player.transform.position;
        playerPosition.y = stateMachine.Monster.transform.position.y;

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
    public override void Exit()
    {
        stateMachine.Monster.anim.SetBool(AnimationHashes.Chase, false);
    }

    public bool CanStartAttack(Monster monster)
    {
        return HasState(stateMachine.Monster.anim, AnimationHashes.Attack1);
    }
    public bool HasState(Animator animator, int stateHash)
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

    public void AttackByRange()
    {
        float distance = Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Monster.player.transform.position);

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
        }
    }

}