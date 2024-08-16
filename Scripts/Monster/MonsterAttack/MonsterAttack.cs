using System.Collections;
using System.Threading;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [HideInInspector]
    public Monster monster;
    private float Destructradius = 2f;
    private float MeleeAttackDistance = 2f;

    public virtual void Awake()
    {
        monster = GetComponent<Monster>();
    }
    
    public Vector2 Direction()
    {
        return monster.isRight ? Vector2.right : Vector2.left;
    }

    public void MeleeAttack() // 근접
    {

        if (monster.attackPatternType == AttackPatternType.First)
        {
             MeleeAttackDistance = monster.canstats.attackDistance[0];
        }
        else if (monster.attackPatternType == AttackPatternType.Second)
        {
             MeleeAttackDistance = monster.canstats.attackDistance[1];
        }
        else if (monster.attackPatternType == AttackPatternType.Third)
        {
             MeleeAttackDistance = monster.canstats.attackDistance[2];
        }

        Vector2 direction = Direction();
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y + monster.Addtransformvalue); // y값을 0.5만큼 올림
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, MeleeAttackDistance, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            if (monster.playerHealth != null)
            {
                if (monster.playerHealth.CurrentHealth > 0)
                {
                    monster.playerHealth.ChangeHealth(-monster.canstats.damage);
                }
            }
        }
    }

    public void DestructAttack() // 자폭
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Destructradius);
        if (hit != null)
        {
            foreach (Collider2D collider in hit)
            {
                if (collider.CompareTag("Player"))
                {
                    monster.playerHealth.ChangeHealth(-monster.canstats.damage);
                }
            }
        }
    }

    public void RushAttack() //돌진 
    {
        Vector2 direction = Direction();
        transform.position = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z);
    }

    public void StraightRangedAttack(GameObject projectile)
    {
        if(GameManager.Instance.pool != null)
        {
            projectile = GameManager.Instance.pool.projectileSpawnFromPool(projectile);
            if (projectile != null)
            {
                projectile.transform.position = monster.RangedAttackpos.position;
                projectile.transform.rotation = monster.RangedAttackpos.rotation;
                Vector2 direction = monster.dir.x > 0 ? Vector2.right : Vector2.left;
                projectile.GetComponent<Bullet>().SetBulletPosition(direction * monster.canstats.speed * 3);
            }
        }
    }

    public void DiagonalRangedAttack(GameObject projectile)
    {
        if (GameManager.Instance.pool != null)
        {
            projectile = GameManager.Instance.pool.projectileSpawnFromPool(projectile);
            if (projectile != null)
            {

                Transform playertransform = monster.player.transform;
                projectile.transform.position = monster.RangedAttackpos.position;

                // 방향 설정 및 발사
                Vector2 direction = monster.dir.x > 0 ? Vector2.right : Vector2.left;
                projectile.GetComponent<ImpFire>().FireProjectile(direction * monster.canstats.speed, playertransform);
            }
        }
    }
}
