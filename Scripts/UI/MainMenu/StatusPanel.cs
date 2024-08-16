using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    MainMenuUI mainMenuUI;
    HealthSystem healthSystem;
    StatHandler statHandler;

    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI mpTxt;
    public TextMeshProUGUI atkTxt;
    public TextMeshProUGUI soulTxt;

    private float currentHp;
    private float maxHp;
    private float currentMp;
    private float maxMp;
    private float atkValue;
    private int hasSoul;

    private void Start()
    {
        statHandler = GameManager.Instance.Player.GetComponent<StatHandler>();
        healthSystem = GameManager.Instance.Player.healthSystem;
        mainMenuUI = GetComponentInParent<MainMenuUI>();
        atkValue = statHandler.CurrentStat.statsSO.damage;
        TextController();
    }

    private void Update()
    {
        TextController();
    }

    public void TextController()
    {
        maxHp = healthSystem.MaxHealth;
        maxMp = healthSystem.MaxMana;

        currentHp = healthSystem.CurrentHealth;
        currentMp = healthSystem.CurrentMana;
        atkValue = statHandler.CurrentStat.statsSO.damage;

        int currentHP = Mathf.RoundToInt(currentHp);
        int currentMP = Mathf.RoundToInt(currentMp);
        int currentAtk = Mathf.RoundToInt(atkValue);

        hpTxt.text = $" H P : {currentHP} / {maxHp}";
        mpTxt.text = $" M P : {currentMP} / {maxMp}";
        atkTxt.text = $" °ø°Ý·Â : {currentAtk}";      
    }
}
