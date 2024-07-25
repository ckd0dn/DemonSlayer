using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("UI")]
    public BossUI bossUI;

    [Header("Room")]
    public Room myRoom;

    protected virtual void Awake()
    {
        bossUI = UIManager.Instance.Show<BossUI>(); 
    }

}
