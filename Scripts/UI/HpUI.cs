using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    Player player;
    TextMeshProUGUI hpText;

    private void Awake()
    {
        hpText = GetComponent<TextMeshProUGUI>(); 
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.healthSystem.OnDamage += UpdateHp;
        player.healthSystem.OnHeal += UpdateHp;
    }

    private void UpdateHp()
    {
        hpText.text = "HP : " + player.healthSystem.CurrentHealth.ToString();
    }
    
}
