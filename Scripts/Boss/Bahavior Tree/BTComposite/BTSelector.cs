// Selector Node 
// 자식 노드를 순차적으로 실행
// 자식 노드가 Success 반환 시 중단 후 Success 반환
// Success 상태인 자식 노드가 없으면 Failure 반환
public class BTSelector : BTNode
{
    public override BTNodeState Evaluate()
    {
        foreach (BTNode node in children)
        {
            BTNodeState result = node.Evaluate();

            if (result == BTNodeState.Success)
            {
                return BTNodeState.Success;
            }
            else if (result == BTNodeState.Running)
            {
                return BTNodeState.Running;
            }
        }

        return BTNodeState.Failure;
    }
}