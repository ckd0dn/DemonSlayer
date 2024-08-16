using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMonster : Monster
{
    public AudioClip BombClip;
    public override void OnEnable()
    {
        base.OnEnable();
        monsterstateMachine.ChangeState(monsterstateMachine.idleState);
        healthSystem.OnDeath += OnBombDeathHandler;
    }

    public void OnBombDeathHandler()
    {
        StartCoroutine(MonsterDeath());
    }

    public override IEnumerator MonsterDeath()
    {
        isDead = true;
        soundManager.PlaySFX(BombClip);
        monstersprite.color = Color.gray;
        anim.SetBool(AnimationHashes.Death, true);
        gameObject.layer = LayerMask.NameToLayer(monsterDeadLayer);
        if (Random.value < dropChance)
        {
            DropItem(transform.position, canstats.monsterClassType);
        }
        healthSystem.OnDeath -= OnDeathHandler;
        yield return StartCoroutine(AnimataionDeath());
    }
    public override IEnumerator AnimataionDeath()
    {
        while (true)
        {
            var animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.IsName("Death"))
            {
                float normalizedtime = animatorStateInfo.normalizedTime % 1;

                if (animatorStateInfo.shortNameHash == AnimationHashes.Death)
                {
       
                    if (normalizedtime >= animationEndTime)
                    {
                        gameObject.SetActive(false);
                        gameObject.layer = LayerMask.NameToLayer(monsterAliveLayer);
                        monstersprite.color = Color.white;
                        isDead = false;
                        break;
                    }
                }
            }
            yield return null;
        }
    }
}
