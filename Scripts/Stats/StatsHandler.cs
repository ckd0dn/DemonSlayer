using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class StatHandler : MonoBehaviour
{
    // 기본 스탯과 버프 스탯들의 능력치를 종합해서 스탯을 계산하는 컴포넌트
    [SerializeField] private Stats baseStat;
    public Stats CurrentStat { get; private set; } = new();

    public List<Stats> statsModifiers = new List<Stats>();

    private readonly float MinAttackDelay = 0.03f;
    private readonly int MinAttackPower = 1;
    private readonly float MinAttackSize = 0.4f;
    private readonly float MinAttackSpeed = 0.1f;

    private readonly float MinSpeed = 0.8f;

    private readonly int MinMaxHealth = 5;
    public event Action StatChange;

    private void Awake()
    {
        SetBaseStats();
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

    private void UpdateCharacterStat()
    {
        ApplyStatModifier(baseStat);

        foreach (Stats stat in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            ApplyStatModifier(stat);
        }
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

        var currentAttack = CurrentStat.statsSO;
        var newAttack = modifier.statsSO;

        currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), MinAttackDelay);
        currentAttack.damage = (int)Mathf.Max(operation(currentAttack.damage, newAttack.damage), MinAttackPower);
    }

    private void UpdateBasicStats(Func<float, float, float> operation, Stats modifier)
    {
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