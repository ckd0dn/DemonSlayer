using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [Header("Stat Data")]
    public MonsterStatsSO data;

    public bool isActing = false;
    public bool isWalking = false;
    [HideInInspector] public bool isDie = false;
    [HideInInspector] public bool isInvincibility = false; 

    [HideInInspector] public HealthSystem healthSystem;
    public Animator Animator { get; set; }
    protected Rigidbody2D _rigidbody;
    protected SpriteRenderer spriteRenderer;
    protected PaintWhite paintWhite;
    protected SoundManager soundManager;
    public BTSelector root;

    public float fixedY;

    [Header("Sound")]
    public AudioClip bgmClip;
    public AudioClip damageClip;
    public AudioClip deathClip;

    [Header("UI")]
    public BossUI bossUI;

    [Header("Room")]
    public Room myRoom;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        Animator = GetComponentInChildren<Animator>();
        paintWhite = GetComponentInChildren<PaintWhite>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        soundManager = SoundManager.Instance;
    }

    protected virtual void Start()
    {
        fixedY = transform.position.y;

        healthSystem.OnDamage += OnDamage;
        healthSystem.OnDeath += OnDeath;
        GameManager.Instance.Player.healthSystem.OnDeath += healthSystem.ResetHealth;
        GameManager.Instance.Player.healthSystem.OnDeath += ResetBoss;

        InitializeBehaviorTree();
        root.Evaluate();
        bossUI = UIManager.Instance.bossUI;
    }

    protected virtual void Update()
    {
        root.Evaluate();
    }

    protected abstract void InitializeBehaviorTree();

    protected abstract BTNodeState MoveToTarget();

    public abstract void Flip(Vector2 dir);

    protected abstract bool CanAct();

    protected abstract bool IsInAttackRange();

    protected abstract bool IsPlayerInRoom();

    protected abstract void ResetBoss();

    protected virtual void OnDamage()
    {
        paintWhite.FlashWhite();
        soundManager.PlaySFX(damageClip);
    }

    protected virtual void OnDeath()
    {
        if (!isDie)
        {
            PlayBgmOnDeath();

            DropItem();

            myRoom.IsBossAlive = false; // 현재방에서 보스 비활성화
            GameManager.Instance.Player.healthSystem.OnDeath -= ResetBoss;

            Animator.SetTrigger("Death");
            isDie = true;
            isActing = true;
            Destroy(gameObject, 2f);
        }
    }

    protected void PlayBgmOnDeath()
    {
        SoundManager.Instance.WaitCurrentBgmAndPlayNext(deathClip, bgmClip);
    }

    public void DropItem( )
    {
        float Addtransformvalue = 0.5f;
        // 소울 드랍
        int dropCount = data.dropSoulCount;
        string dropItemName = data.dropItemName; 

        for (int i = 0; i < dropCount; i++)
        {
            GameObject soulObj = GameManager.Instance.pool.SpawnFromPool("Soul");
            Soul soul = soulObj.GetComponent<Soul>();
            soul.Drop(transform.position);
        }

        // 유물 드랍
        GameObject relics = GameManager.Instance.pool.SpawnFromPool(dropItemName);
        if (relics != null)
        {
            relics.transform.position = new Vector3(transform.position.x, transform.position.y + Addtransformvalue, transform.position.z);
            relics.transform.rotation = Quaternion.identity;
        }
    }
}
