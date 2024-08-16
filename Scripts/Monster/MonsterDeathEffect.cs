using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathEffect : MonoBehaviour
{
    public Animator monsterDeathAnim;

    private void Awake()
    {
        monsterDeathAnim = GetComponent<Animator>();    
    }

    public void ShowAnimation()
    {
        monsterDeathAnim.enabled = true;
    }
}
