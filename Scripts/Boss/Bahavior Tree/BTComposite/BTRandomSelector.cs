using System;
using System.Collections.Generic;

// RandomSelector Node 
// 자식 노드를 무작위로 섞고
// Selector와 같은 방식으로 동작
public class BTRandomSelector : BTSelector
{
    private Random random = new Random();

    public override BTNodeState Evaluate()
    {
        Suffle(children);        

        return base.Evaluate();
    }

    private void Suffle(List<BTNode> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            BTNode node = list[k];
            list[k] = list[i];
            list[i] = node;
        }
    }
}