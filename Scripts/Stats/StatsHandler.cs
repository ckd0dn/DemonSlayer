using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class StatHandler : MonoBehaviour
{
    // 기본 스탯과 버프 스탯들의 능력치를 종합해서 스탯을 계산하는 컴포넌트
    public Stats baseStat;
    public PlayerAttack playerAttack;
    public Stats CurrentStat { get; private set; } = new();

    public List<Stats> statsModifiers = new List<Stats>();

    private readonly float MinAttackDelay = 0.03f;
    private readonly int MinAttackPower = 1;
    private readonly int MinMaxHealth = 5;
    public event Action StatChange;

    private void Awake()
    {
        playerAttack = GameManager.Instance.Player.PlayerAttack;
        SetBaseStats();
    }

    private void Start()
    {
        UpdateCharacterStat();
    }
    private void SetBaseStats()
    {
        if (baseStat.statsSO != null)
        {
            baseStat.statsSO = Instantiate(baseStat.statsSO);
            CurrentStat.statsSO = Instantiate(baseStat.statsSO);
            CurrentStat.maxHealth = baseStat.statsSO.hp;
        }
    }

    public void UpdateCharacterStat()
    {
        ApplyStatModifier(baseStat);

        foreach (Stats stat in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            ApplyStatModifier(stat);
        }
        playerAttack.UpdatePlayerAtk();
    }

    public void AddStatModifier(Stats modifier)
    {
        statsModifiers.Add(modifier);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(int index)
    {
        //statsModifiers.Remove(statModifier);
        statsModifiers.RemoveAt(index);
        UpdateCharacterStat();
        
    }
    private void ApplyStatModifier(Stats modifier)
    {
        Func<float, float, float> operation = Operation(modifier.statsChangeType);

        UpdateBasicStats(operation, modifier); //
        UpdateAttackStats(operation, modifier);
    }

    private Func<float, float, float> Operation(StatsChangeType statsChangeType)
    {
        return statsChangeType switch
        {
            StatsChangeType.Add => (current, change) => current + change,
            StatsChangeType.Multiple => (current, change) => current * change,
            _ => (current, change) => change
        };
    }


    private void UpdateAttackStats(Func<float, float, float> operation, Stats modifier)
    {
        if (CurrentStat.statsSO == null || modifier.statsSO == null) return;

        CurrentStat.statsSO.delay = Mathf.Max(operation(CurrentStat.statsSO.delay, modifier.statsSO.delay), MinAttackDelay);
        CurrentStat.statsSO.damage = Mathf.Max((int)operation(CurrentStat.statsSO.damage, modifier.statsSO.damage), MinAttackPower);

    }

    private void UpdateBasicStats(Func<float, float, float> operation, Stats modifier)
    {
        if (CurrentStat.statsSO == null || modifier.statsSO == null) return;

        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.statsSO.hp), MinMaxHealth);
        OnUpdateBasicStat();
    }
    public void OnUpdateBasicStat()
    {
        StatChange?.Invoke();
    }
    //private void UpdateRangedAttackStats(Func<float, float, float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    //{
    //    currentRanged.multipleProjectilesAngle = operation(currentRanged.multipleProjectilesAngle, newRanged.multipleProjectilesAngle);
    //    currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
    //    currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
    //    currentRanged.numberOfProjectilesPerShot = Mathf.CeilToInt(operation(currentRanged.numberOfProjectilesPerShot, newRanged.numberOfProjectilesPerShot));
    //    currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);
    //}

}