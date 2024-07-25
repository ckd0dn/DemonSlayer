using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public MonsterObjectPool monsterObjectPool;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && monsterObjectPool != null)
        {
            monsterObjectPool.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && monsterObjectPool != null)
        {
            monsterObjectPool.enabled = false;
        }
    }
}
