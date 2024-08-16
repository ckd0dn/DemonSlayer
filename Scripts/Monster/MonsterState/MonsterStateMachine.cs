using UnityEngine;
public class MonsterStateMachine : StateMachine
{
    public Monster Monster { get; }
    public IdleState idleState { get;  }
    public ChasingState chasingState { get;}
    public AttackingState attackingState { get; }

    //플라이 몬스터 
    public FlyingIdleState flyingidleState { get; }
    public FlyingChaseingState flyingChaseingState { get; }

    public BombIdleState BombIdleState { get; }
    public BombChaseingState BombChaseingState { get; }
    public BombAttackingState BombAttackingState { get; }

    public WalkIdleState WalkIdleState { get; }

    public PracticeMonsterIdleState practiceMonsterIdleState { get; }
    public MonsterStateMachine(Monster monster)
    {
        this.Monster = monster;

        switch (monster.canstats.monsterType)
        {
            case MonsterType.Flying:
                idleState = new FlyingIdleState(this);
                chasingState = new FlyingChaseingState(this);
                attackingState = new AttackingState(this);
                break;
            case MonsterType.Destruct:
                idleState = new BombIdleState(this);
                chasingState = new BombChaseingState(this);
                attackingState = new BombAttackingState(this);
                break;
            case MonsterType.Walk:
                idleState = new WalkIdleState(this);
                chasingState = new ChasingState(this);
                attackingState = new AttackingState(this);
                break;
            case MonsterType.Practice:
                idleState = new PracticeMonsterIdleState(this);
                break;
            default:
                idleState = new IdleState(this);
                chasingState = new ChasingState(this);
                attackingState = new AttackingState(this);
                break;
        }        

    }

}
