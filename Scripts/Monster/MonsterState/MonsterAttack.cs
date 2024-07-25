using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack
{
    private MonsterController monsterController;
    private Transform monstertransform;
    
    public MonsterAttack(MonsterController controller)
    {
        monsterController = controller;
        monstertransform = monsterController.transform;
    }
    

    public void Attack()
    {
        Vector2 direction = monsterController.Direction();
        Vector2 startPosition = new Vector2(monstertransform.position.x, monstertransform.position.y + 0.5f); // y값을 0.5만큼 올림
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, 3f, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            if (monsterController.playerHealth != null)
            {
                if (monsterController.playerHealth.CurrentHealth > 0)
                {
                    monsterController.playerHealth.ChangeHealth(-monsterController.canstats.damage);
                }
            }
        }
    }

    public void CloseAttack()
    {
        Vector2 direction = monsterController.Direction();
        monstertransform.position = new Vector3(monstertransform.position.x + direction.x, monstertransform.position.y, monstertransform.position.z);
    }

    public void RangedAttack()
    {
        if (monsterController.ObjectPool != null)
        {
            GameObject bullet = monsterController.ObjectPool.SpawnFromPool("Bullet");
            if (bullet != null)
            {
                bullet.transform.position = monsterController.RangedAttackpos.position;
                Vector2 direction = monsterController.dir.x > 0 ? Vector2.right : Vector2.left;
                bullet.GetComponent<Bullet>().SetBulletPosition(direction * monsterController.canstats.speed * 3);
                //bullet.GetComponent<Rigidbody2D>().velocity = direction * monsterController.canstats.speed * 3;
            }
        }
    }
}
//monsterController.IsLookPlayer(monsterController.gameObject.transform);