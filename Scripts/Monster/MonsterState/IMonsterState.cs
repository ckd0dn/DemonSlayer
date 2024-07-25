using System.Collections;

public interface IMonsterState
{
    IEnumerator Execute(MonsterController monster);
}