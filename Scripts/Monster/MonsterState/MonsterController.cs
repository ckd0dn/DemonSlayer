using System.Collections;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Chasing,
    Attack,
    Death,
}

public class MonsterController : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;

    //몬스터 상태
    public MonsterState currentState;
    private Coroutine currentCoroutine;
    public SpriteRenderer monstersprite;
    public PaintWhite paintWhite;
    private Rigidbody2D rb;

    IMonsterState idleState = new IdleState();
    IMonsterState chasingState = new ChasingState();
    IMonsterState attackingState = new AttackingState();

    // 몬스터 스탯 설정
    [HideInInspector]
    public MonsterStatsSO canstats;

    public MonsterStatsSO stats;
    public HealthSystem healthSystem;
    public HealthSystem playerHealth;
    public SoundManager soundManager;
    MonsterObjectPool monsterObjectPool;

    //몬스터 공격
    public Transform RangedAttackpos;
    public Player player;
    public float knockbackpower;
    GameObject bullet;
    private MonsterAttack monsterAttack;
    public ObjectPool ObjectPool;

    //몬스터 이동
    public bool isRight;
    public Vector3 dir;
    float distance;
    //bool TouchWall;
    public bool ChasingDelay;

    //몬스터 아이템 드랍
    [SerializeField] private float dropChance = 0.99f;
    [SerializeField] private int minDropCount = 1;
    [SerializeField] private int maxDropCount = 1;

    // 몬스터 사운드
    public AudioClip hitClip;
    public AudioClip deathClip;

    private void Awake()
    {
        InitializeStats();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        anim = GetComponent<Animator>();
        monstersprite = GetComponent<SpriteRenderer>();
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        paintWhite = GetComponent<PaintWhite>();
        ObjectPool = GetComponent<ObjectPool>();
        soundManager = SoundManager.Instance;
        knockbackpower = 5f;
        monsterAttack = new MonsterAttack(this);
    }

    public void InitializeStats()
    {
        canstats = Instantiate(stats);
        
    }
    private void OnEnable()
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;
            playerHealth = player.healthSystem;
        }

        currentState = MonsterState.Idle;
        isRight = true;
        if (isRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        FSM();
        healthSystem.OnDamage += MonsterHit;
        healthSystem.OnDeath += OnDeathHandler; 
        //이벤트 구독은 람다식으로 하면, 새로운 델리게이트를 하나 더 만든다. 그러므로 이벤트 구독은 람다식으로 하지 말자 
    }


    private void OnDisable()
    {
        healthSystem.OnDamage -= MonsterHit;
        healthSystem.OnDeath -= OnDeathHandler;
    }
    private void OnDeathHandler()
    {
        StartCoroutine(MonsterDeath());
    }
    public void MonsterHit()
    {
        if (currentState == MonsterState.Attack)
        {
            HitEffect();
        }
        else
        {
            anim.SetTrigger(AnimationHashes.Hit);
            HitEffect();
        }

        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
        if (direction.x < 0.5f)
        {
            direction.x = isRight ? 2 : -2;
        }
        rb.AddForce(direction * -1f * knockbackpower, ForceMode2D.Impulse);
        //if (!TouchWall)
        
    }
    public void HitEffect()
    {
        paintWhite.FlashWhite();
        soundManager.PlaySFX(hitClip); 
    }

    private IEnumerator MonsterDeath()
    {
        player.isEnemyDead = true;
        currentState = MonsterState.Idle;

        ResetAnimationBools();
        soundManager.PlaySFX(deathClip); 
        currentState = MonsterState.Death;
        monstersprite.color = Color.gray;
        anim.SetBool(AnimationHashes.Death, true);
        if (Random.value < dropChance)
        {
            DropItem(transform.position, canstats.monsterType);
        }
        healthSystem.OnDeath -= OnDeathHandler;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        monstersprite.color = Color.white;
        player.isEnemyDead = false;
        yield break;
        
    }



    public void ChangeState(MonsterState newState)
    {
        if (currentState == newState) return;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        ResetAnimationBools();
        currentState = newState;
        FSM();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "endpoint")
        {
            if (isRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isRight = true;
            }
        }
    }

    //StartCoroutine(Delay());
    //ChangeState(MonsterState.Idle);
    //TouchWall = true;
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "endpoint")
    //    {
    //        TouchWall = false;
    //    }
    //}

    //IEnumerator Delay()
    //{
    //    ChasingDelay = true;
    //    yield return new WaitForSeconds(2f);
    //    ChasingDelay = false;
    //}

    //public void MonsterChasingTrigger()
    //{
    //    if (!ChasingDelay)
    //    {
    //        ChangeMonsterStates();
    //    }
    //}

    //public void ChangeMonsterStates()
    //{
    //    distance = Vector3.Distance(transform.position, player.transform.position);

    //    if (distance < canstats.range)
    //    {
    //        ChangeState(MonsterState.Chasing);
    //    }
    //}

    public Vector2 Direction()
    {
        return isRight ? Vector2.right : Vector2.left;
    }
    #region 공격,원거리공격
    public void Attack()
    {
        monsterAttack.Attack();
    }

    public void CloseAttack()
    {
        monsterAttack.CloseAttack();
    }

    public void RangedAttack()
    {
        monsterAttack.RangedAttack();
    }
    #endregion

    public void IsLookPlayer(Transform transform)
    {
        dir = player.transform.position - transform.position;
        if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            isRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            isRight = false;
        }
    }

    void FSM()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                anim.SetBool(AnimationHashes.Idle, true);
                currentCoroutine = StartCoroutine(idleState.Execute(this));
                break;
            case MonsterState.Chasing:
                anim.SetBool(AnimationHashes.Chase, true);
                currentCoroutine = StartCoroutine(chasingState.Execute(this));
                break;
            case MonsterState.Attack:
                anim.SetBool(AnimationHashes.Attack, true);
                currentCoroutine = StartCoroutine(attackingState.Execute(this));
                break;
        }
    }

    private void ResetAnimationBools()
    {
        anim.SetBool(AnimationHashes.Idle, false);
        anim.SetBool(AnimationHashes.Chase, false);
        anim.SetBool(AnimationHashes.Attack, false);
        anim.SetBool(AnimationHashes.Death, false);
    }

    public void PauseAnimation()
    {
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
        anim.speed = 0;
    }

    public void ResumeAnimation()
    {
        anim.speed = 1;
    }

    public void DropItem(Vector3 postion, MonsterType monstertype)
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
        if (monstertype == MonsterType.Elite)
        {
            GameObject relics = GameManager.Instance.pool.SpawnFromPool("Relics");
            if (relics != null)
            {
                relics.transform.position = new Vector3(postion.x, postion.y + 0.5f, postion.z);
                relics.transform.rotation = Quaternion.identity;
            }
        }
    }

}
