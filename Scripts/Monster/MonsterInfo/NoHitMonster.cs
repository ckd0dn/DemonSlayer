using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHitMonster : Monster
{
    public override void MonsterReset()
    {
        gameObject.SetActive(false);
        healthSystem.ResetHealth();
        monstersprite.color = Color.white;
        isDead = false;
    }

}
