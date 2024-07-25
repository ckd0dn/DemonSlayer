using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float healthChangeDelay = .5f;
    [SerializeField] private int DamageClipIdx;

    private StatHandler statsHandler;
    public float timeSinceLastChange = float.MaxValue;
    [HideInInspector] public bool isInvincibility = false;

    // 체력이 변했을 때 할 행동들을 정의하고 적용 가능
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    // public event Action OnInvincibilityEnd;
    public event Action<float> OnHealthChanged;

    // 마나 
    public event Action OnRefillMana;
    public event Action OnConsumeMana;
    public event Action OnManaZero;

    public LayerMask layerMask;

    [SerializeField] public float CurrentHealth { get => data.CurrentHealth; set => data.CurrentHealth = value; }
    [SerializeField] public float CurrentMana { get => data.CurrentMana; set => data.CurrentMana = value; }
    [SerializeField] public float CurrentStamina { get => data.CurrentStamina; set => data.CurrentStamina = value; }

    // get만 구현된 것처럼 프로퍼티를 사용하는 것
    public float MaxHealth { get => data.MaxHealth; set => data.MaxHealth = value; }
    public float MaxMana { get => data.MaxMana; set => data.MaxMana = value; }
    public float MaxStamina { get => data.MaxStamina; set => data.MaxStamina = value; }

    public HealthSystemData data;

    private void Awake()
    {
        SetStats();
    }

    public void SetStats()
    {
        statsHandler = GetComponent<StatHandler>();
        CurrentHealth = statsHandler.CurrentStat.maxHealth;

        UpdateStats();
    }
    private void Start()
    {
        statsHandler.StatChange += UpdateStats;
    }

    public void UpdateStats()
    {
        MaxHealth = statsHandler.CurrentStat.maxHealth;

        if (statsHandler.CurrentStat.statsSO is PlayerStatsSO)
        {
            //CurrentHealth = statsHandler.CurrentStat.statsSO.hp;
            PlayerStatsSO playerStatsSO = (PlayerStatsSO)statsHandler.CurrentStat.statsSO;

            if (playerStatsSO != null)
            {
                MaxMana = playerStatsSO.mana;
                MaxStamina = playerStatsSO.stamina;

                CurrentMana = playerStatsSO.mana;
                CurrentStamina = playerStatsSO.stamina;
            }
        }
        
    }


    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            //if (timeSinceLastChange >= healthChangeDelay)
            //{
            //    OnInvincibilityEnd?.Invoke();
            //}
        }
        ChangeMana(0.01f);
    }

    public bool ChangeHealth(float change)
    {
        if (isInvincibility && change <= 0)
        {
            return false;
        }

        if (timeSinceLastChange < healthChangeDelay && transform.CompareTag("Player"))
        {
            return false;
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        // 최솟값을 0, 최댓값을 MaxHealth로 하는 구문.
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        // CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth; 와 같다


        if (CurrentHealth <= 0f)
        {
            CallHealthChanged();
            CallDeath();
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
        }

        CallHealthChanged();

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public bool ChangeMana(float change)
    {
        CurrentMana += change;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);

        if (CurrentMana <= 0f)
        {
            OnManaZero?.Invoke();
            return true;
        }

        if (change >= 0)
        {
            OnRefillMana?.Invoke();
        }
        else
        {
            OnConsumeMana?.Invoke();
        }

        return true;
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;

        if (statsHandler.CurrentStat.statsSO is PlayerStatsSO)
        {
            CurrentMana = MaxMana;
            CurrentStamina = MaxStamina;
        }
    }
}