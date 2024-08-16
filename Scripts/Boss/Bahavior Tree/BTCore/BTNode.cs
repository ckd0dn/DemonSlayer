using System.Collections.Generic;

// 각 노드는 진행중, 성공, 실패 3가지의 상태를 가진다
public enum BTNodeState
{
    Running,
    Success,
    Failure
}

public abstract class BTNode
{
    // 트리 선언
    public List<BTNode> children = new List<BTNode>();

    // 자식 노드 추가 함수
    public void AddChild(BTNode node)
    {
        children.Add(node);
    }
    
    // 노드의 상태를 반환하는 함수
    public virtual BTNodeState Evaluate()
    {
        return BTNodeState.Failure;
    }
}