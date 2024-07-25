using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float damageAmount;

    protected void ApplyDamage()
    {
        //플레이어에게 데미지를 입힘
        GameManager.Instance.Player.healthSystem.ChangeHealth(-damageAmount);
    }
}
