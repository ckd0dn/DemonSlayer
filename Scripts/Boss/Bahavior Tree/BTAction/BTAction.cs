using System;

// Leaf Node
// 델리게이트로 함수를 받아서 기능 수행
// 수행 후 노드의 상태를 반환
public class BTAction : BTNode
{
    Func<BTNodeState> action = null;

    public BTAction(Func<BTNodeState> action)
    {
        this.action = action;
    }

    public override BTNodeState Evaluate()
    {        
        return action();
    }
}