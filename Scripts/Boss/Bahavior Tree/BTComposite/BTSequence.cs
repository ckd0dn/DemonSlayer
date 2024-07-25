// Sequence Node
// 모든 자식 노드를 순차적으로 실행
// 자식 노드가 Failure 반환 시 중단 후 Failure 반환
public class BTSequence : BTNode
{
    public override BTNodeState Evaluate()
    {
        bool isChildInProgress = false;

        foreach (BTNode node in children)
        {
            BTNodeState result = node.Evaluate();

            if (result == BTNodeState.Failure)
            {
                return BTNodeState.Failure;
            }
            else if (result == BTNodeState.Running)
            {
                isChildInProgress = true;
            }
        }
        return isChildInProgress ? BTNodeState.Running : BTNodeState.Success;
    }
}