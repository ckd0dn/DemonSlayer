using UnityEngine;

public class MonsterBaseState : IState // 모든 상태가 들고있는 
{
    protected MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public void HandleInput()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
    }

    public void IsLookPlayer(Transform transform)
    {
        stateMachine.Monster.dir = stateMachine.Monster.player.transform.position - transform.position;
        if (stateMachine.Monster.dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            stateMachine.Monster.isRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            stateMachine.Monster.isRight = false;
        }
    }

}
