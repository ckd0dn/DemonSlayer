using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Monster : MonoBehaviour // 몬스터의 스탯, 
{
    [HideInInspector]
    public Animator anim;
    private MonsterDeathEffect monsterDeathEffect;

    // 몬스터 스탯 설정
    [HideInInspector]
    public MonsterStatsSO canstats;
    public MonsterStatsSO stats;
    public HealthSystem healthSystem;
    public HealthSystem playerHealth;
    public SoundManager soundManager;

    // 몬스터 공격
    public Transform RangedAttackpos;
    [HideInInspector]
    public Player player;
    public float knockbackpower = 7f;
    public AttackType firstAttackType = AttackType.MeleeAttack;
    public AttackType secondAttackType = AttackType.MeleeAttack;
    public AttackPatternType attackPatternType;

    // 몬스터 이동 및 위치값
    public bool isRight;
    public Vector3 dir;
    public float Addtransformvalue = 0.5f;

    // 몬스터 아이템 드랍
    public float dropChance = 0.99f;
    [SerializeField] private int minDropCount = 1;
    [SerializeField] private int maxDropCount = 1;

    // 몬스터 사운드
    public AudioClip hitClip;
    public AudioClip deathClip;

    //몬스터 상태머신
    public MonsterStateMachine monsterstateMachine;

    //몬스터 상태
    public SpriteRenderer monstersprite;
    public PaintWhite paintWhite;
    public Rigidbody2D rb;
    public bool isDead = false;
    public bool isAttacking = false;
    public float attackDelayTime = 3f;
    WaitForSeconds attackDelayTimeWaitSeconds;
    public string monsterDeadLayer = "EnemyDead";
    public string monsterAliveLayer = "Enemy";
    public float animationEndTime;
    bool HitAnimationPlaying;
    protected virtual void Awake()
    {
        InitializeStats();
        InitializeComponents();
        animationEndTime = 0.94f;
        HitAnimationPlaying = false;
    }

    public virtual void InitializeComponents()
    {
        anim = GetComponent<Animator>();
        monsterDeathEffect = GetComponentInChildren<MonsterDeathEffect>();
        monstersprite = GetComponent<SpriteRenderer>();
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        paintWhite = GetComponent<PaintWhite>();
        soundManager = SoundManager.Instance;
        monsterstateMachine = new MonsterStateMachine(this);
        attackDelayTimeWaitSeconds = new WaitForSeconds(attackDelayTime);

    }

    public void InitializeStats()
    {
        canstats = Instantiate(stats);
    }

    public virtual void OnEnable()
    {
        monsterstateMachine.ChangeState(monsterstateMachine.idleState);

        if (player == null)
        {
            player = GameManager.Instance.Player;
            playerHealth = player.healthSystem;
        }
        isRight = true;
        if (isRight)
        {
            transform.eulerAngles = Vector3.zero;
        }

        healthSystem.OnDamage += MonsterHit;
        healthSystem.OnDeath += OnDeathHandler;

    }

    private void OnDisable()
    {
        healthSystem.OnDamage -= MonsterHit;
        healthSystem.OnDeath -= OnDeathHandler;

        isAttacking = false;
        StopAllCoroutines();
    }

    private void Start()
    {
        healthSystem.OnDeath += GameManager.Instance.cameraUtil.Shake;
    }

    public void OnDeathHandler()
    {
        StartCoroutine(MonsterDeath());
    }


    public virtual void MonsterHit()
    {
        var animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Attack1") && !animatorStateInfo.IsName("Attack2"))
        {
            HitTirggerToggle();
            HitEffect();
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            if (direction.x < 0.5f)
            {
                direction.x = isRight ? 2 : -2;
            }
            rb.AddForce(direction * -1f * knockbackpower, ForceMode2D.Impulse);
        }
        else
        {
            HitEffect();
        }
    }
    void HitTirggerToggle()
    {
        if (!HitAnimationPlaying)
        {
            anim.SetTrigger("Hit");
            HitAnimationPlaying = true;

        }

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= animationEndTime)
        {
            anim.ResetTrigger("Hit");
            HitAnimationPlaying = false;
        }
    }
    public void HitEffect()
    {
        paintWhite.FlashWhite();
        soundManager.PlaySFX(hitClip);
    }

    public virtual IEnumerator MonsterDeath()
    {
        isDead = true;
        soundManager.PlaySFX(deathClip);
        monstersprite.color = Color.gray;
        anim.SetBool(AnimationHashes.Death, true);
        gameObject.layer = LayerMask.NameToLayer(monsterDeadLayer);
        if (Random.value < dropChance)
        {
            DropItem(transform.position, canstats.monsterClassType);
        }
        if (monsterDeathEffect != null) monsterDeathEffect.ShowAnimation();
        yield return StartCoroutine(AnimataionDeath());
    }

    public virtual IEnumerator AnimataionDeath()
    {
        while (true)
        {
            var animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("Death"))
            {
                float normalizedtime = animatorStateInfo.normalizedTime % 1;
                if (animatorStateInfo.shortNameHash == AnimationHashes.Death)
                {
                    if (normalizedtime >= animationEndTime)
                    {
                        MonsterReset();
                        break;
                    }
                }
            }
            yield return null;
        }
    }
    public virtual void MonsterReset()
    {
        gameObject.SetActive(false);
        if (monsterDeathEffect != null) monsterDeathEffect.monsterDeathAnim.enabled = false;
        healthSystem.ResetHealth();
        gameObject.layer = LayerMask.NameToLayer(monsterAliveLayer);
        monstersprite.color = Color.white;
        isDead = false;
    }
    private void FixedUpdate()
    {
        if (isDead) return;
        if (GameManager.Instance.MonsterMove)
        {
            anim.enabled = true;
            monsterstateMachine.Update();            
        }
        else
        {
            anim.enabled = false;
            return;
        }
    }

    public IEnumerator AttackDelayTime()
    {
        isAttacking = true;
        yield return attackDelayTimeWaitSeconds;
        isAttacking = false;
    }

    public void StartAttackDelay()
    {
        StartCoroutine(AttackDelayTime());
    }

    public void DropItem(Vector3 postion, MonsterClassType monstertype)
    {
        // 소울 드랍
        int dropCount = Random.Range(minDropCount, maxDropCount + 1);

        for (int i = 0; i < dropCount; i++)
        {
            GameObject soulObj = GameManager.Instance.pool.SpawnFromPool("Soul");
            Soul soul = soulObj.GetComponent<Soul>();
            soul.Drop(postion);
        }

        // 유물 드랍
        if (monstertype == MonsterClassType.Elite)
        {
            GameObject relics = GameManager.Instance.pool.SpawnFromPool("Relics");
            if (relics != null)
            {
                relics.transform.position = new Vector3(postion.x, postion.y + Addtransformvalue, postion.z);
                relics.transform.rotation = Quaternion.identity;
            }
        }
    }

    public void Animationbool()
    {
        anim.SetBool(AnimationHashes.Attack1, false);
        anim.SetBool(AnimationHashes.Attack2, false);
        anim.SetBool(AnimationHashes.Attack3, false);
        anim.SetBool(AnimationHashes.Idle, false);
        anim.SetBool(AnimationHashes.Chase, false);
        anim.SetBool(AnimationHashes.Death, false);
    }

}
