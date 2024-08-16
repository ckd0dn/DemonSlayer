using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMonster : Monster
{
    public float HitCount;
    private void Start()
    {
        HitCount = 0;
    }
    public override void MonsterHit()
    {
        base.MonsterHit();
        HitCount++;
    }
}
