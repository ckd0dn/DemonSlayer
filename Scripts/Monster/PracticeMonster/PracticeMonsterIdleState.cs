using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMonsterIdleState : IdleState
{

    public PracticeMonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Update()
    {
        isStopping = true;

        if (isStopping)
        {
            MonsterStop();
        }
    }

}
