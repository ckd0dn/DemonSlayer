using System.Collections;
using UnityEngine;

public class IceGolemAttack : MonsterAttack
{
    public GameObject IceGolemEffet;
    public BoxCollider2D AttackCollider;
    public override void Awake()
    {
        base.Awake();
        IceGolemEffet.SetActive (false);

    }

    public void MeleeWideAttack()
    {
        IceGolemEffet.SetActive(true);
        Collider2D[] hit = Physics2D.OverlapBoxAll(AttackCollider.transform.position, AttackCollider.size, 0);

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

        StartCoroutine(EffectFalse());
    }
    IEnumerator EffectFalse()
    {
        yield return new WaitForSeconds(0.4f);
        IceGolemEffet.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(AttackCollider.transform.position, AttackCollider.size);
    //}
}
