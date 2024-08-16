using System.Collections;
using UnityEngine;

public class DemonSlime : Boss
{
    [Header("Slime Form")]
    public GameObject slimeForm;
    public Animator slimeAnimator;
    public MonsterStatsSO slimeData;

    [Header("Demon Form")]
    public GameObject demonForm;
    public Animator demonAnimator;
    public MonsterStatsSO demonData;

    public DemonSlimeSkills skills;
    public GameObject currentForm;
    public bool isTransformed = false;

    public AudioClip bossBgm;

    protected override void Awake()
    {
        soundManager = SoundManager.Instance;
        slimeAnimator = slimeForm.GetComponentInChildren<Animator>();
        demonAnimator = demonForm.GetComponentInChildren<Animator>();
        currentForm = slimeForm;
        SetComponents();
    }

    protected override void Start()
    {
        fixedY = transform.position.y;
        data = slimeData;

        slimeForm.GetComponent<HealthSystem>().OnDamage += OnDamage;
        demonForm.GetComponent<HealthSystem>().OnDamage += OnDamage;
        demonForm.GetComponent<HealthSystem>().OnDeath += OnDeath;

        GameManager.Instance.Player.healthSystem.OnDeath += slimeForm.GetComponent<HealthSystem>().ResetHealth;
        GameManager.Instance.Player.healthSystem.OnDeath += demonForm.GetComponent<HealthSystem>().ResetHealth;
        GameManager.Instance.Player.healthSystem.OnDeath += ResetBoss;

        InitializeBehaviorTree();
        root.Evaluate();
        bossUI = UIManager.Instance.bossUI;
    }

    protected override void Update()
    {
        base.Update();
        if(!myRoom.IsBossAlive)
        {
            Destroy(gameObject);
        }
    }

    private void SetComponents()
    {
        _rigidbody = currentForm.GetComponent<Rigidbody2D>();
        healthSystem = currentForm.GetComponent<HealthSystem>();
        Animator = currentForm.GetComponentInChildren<Animator>();
        paintWhite = currentForm.GetComponentInChildren<PaintWhite>();
        spriteRenderer = currentForm.GetComponentInChildren<SpriteRenderer>();        

        skills = currentForm.GetComponentInChildren<DemonSlimeSkills>();
    }

    #region BT Setting
    protected override void InitializeBehaviorTree()
    {
        root = new BTSelector();

        BTSequence transformationSequence = CreateTransformationSequence();
        BTSequence slimeFormSequence = new BTSequence();

        BTSelector slimeActionSelector = CreateSlimeActionSelector();

        root.AddChild(transformationSequence);
        root.AddChild(slimeFormSequence);

        slimeFormSequence.AddChild(new BTCondition(IsSlimeForm));
        slimeFormSequence.AddChild(slimeActionSelector);
    }

    private void InitializeDemonFormBT()
    {
        root = new BTSelector();

        BTSequence demonFormSequence = new BTSequence();
        BTSelector demonActionSelector = CreateDemonActionSelector();

        root.AddChild(demonFormSequence);

        demonFormSequence.AddChild(new BTCondition(IsDemonForm));
        demonFormSequence.AddChild(demonActionSelector);
    }

    #region SlimeForm
    private BTSelector CreateSlimeActionSelector()
    {
        BTSelector slimeActionSelector = new BTSelector();
                
        BTSequence slimeMovemnetSequence = CreateSlimeFormMovementSequence();

        slimeActionSelector.AddChild(slimeMovemnetSequence);

        return slimeActionSelector;
    }

    private BTSequence CreateTransformationSequence()
    {
        BTSequence transformationSequence = new BTSequence();

        transformationSequence.AddChild(new BTCondition(CanTransformation));
        transformationSequence.AddChild(new BTAction(skills.TransformAction));

        return transformationSequence;
    }
    #endregion

    #region DemonForm
    private BTSelector CreateDemonActionSelector()
    {
        BTSelector demonActionSelector = new BTSelector();

        BTSequence demonRangedAttackSequence = CreateDemonRangedAttackSequence();
        BTSequence demonMeleeAttackSequence = CreateDemonMeleeAttackSequence();
        BTSequence demonMovementSequence = CreateMovementSequence();

        demonActionSelector.AddChild(demonRangedAttackSequence);
        demonActionSelector.AddChild(demonMeleeAttackSequence);
        demonActionSelector.AddChild(demonMovementSequence);

        return demonActionSelector;
    }
        
    private BTSequence CreateDemonRangedAttackSequence()
    {
        BTSequence demonRangedAttackSequence = new BTSequence();
        BTRandomSelector demonSkillRandomSelector = new BTRandomSelector();

        demonSkillRandomSelector.AddChild(new BTAction(skills.JumpSlamAction));
        demonSkillRandomSelector.AddChild(new BTAction(skills.ChargeSmashAction));

        demonRangedAttackSequence.AddChild(new BTCondition(CanAct));
        demonRangedAttackSequence.AddChild(new BTCondition(IsOutAttackRange));
        demonRangedAttackSequence.AddChild(demonSkillRandomSelector);

        return demonRangedAttackSequence;
    }

    private BTSequence CreateDemonMeleeAttackSequence()
    {
        BTSequence demonMeleeAttackSequence = new BTSequence();
        BTRandomSelector demonSkillRandomSelector = new BTRandomSelector();

        demonSkillRandomSelector.AddChild(new BTAction(skills.SmashAction));
        demonSkillRandomSelector.AddChild(new BTAction(skills.BreathAction));
        demonSkillRandomSelector.AddChild(new BTAction(skills.FireBallAction));

        demonMeleeAttackSequence.AddChild(new BTCondition(CanAct));
        demonMeleeAttackSequence.AddChild(new BTCondition(IsInAttackRange));
        demonMeleeAttackSequence.AddChild(demonSkillRandomSelector);

        return demonMeleeAttackSequence;
    }

    #endregion

    private BTSequence CreateMovementSequence()
    {
        BTSequence movementSequence = new BTSequence();
        movementSequence.AddChild(new BTCondition(CanAct));
        movementSequence.AddChild(new BTAction(MoveToTarget));

        return movementSequence;
    }

    private BTSequence CreateSlimeFormMovementSequence()
    {
        BTSequence movementSequence = new BTSequence();
        movementSequence.AddChild(new BTCondition(CanAct));
        movementSequence.AddChild(new BTAction(SlimeFormMovement));

        return movementSequence;
    }
    #endregion

    protected override BTNodeState MoveToTarget()
    {
        if (IsInDetectRange())
        {
            if (!IsInAttackRange())
            {
                Vector3 targetPosition = GameManager.Instance.Player.transform.position;
                targetPosition.y = fixedY;

                isWalking = true;
                Animator.SetBool("Walk", true);

                currentForm.transform.position = Vector2.MoveTowards(currentForm.transform.position, targetPosition, data.speed * Time.deltaTime);
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
                isWalking = false;
                Animator.SetBool("Walk", false);
            }
        }

        Flip(GameManager.Instance.Player.transform.position - currentForm.transform.position);
        return BTNodeState.Running;
    }

    private BTNodeState SlimeFormMovement()
    {
        if(IsInDetectRange())
        {
            Vector3 playerPosition = GameManager.Instance.Player.transform.position;
            Vector3 playerDirection = playerPosition - currentForm.transform.position;
            Vector3 fleeDirection = -playerDirection.normalized * 5f;

            Vector3 targetPosition = currentForm.transform.position + fleeDirection;
            targetPosition.y = fixedY;

            isWalking = true;
            Animator.SetBool("Walk", true);

            currentForm.transform.position = Vector2.MoveTowards(currentForm.transform.position, targetPosition, data.speed * Time.deltaTime);
        }
        else
        {
            isWalking = false;
            Animator.SetBool("Walk", false);
        }

        Flip(currentForm.transform.position - GameManager.Instance.Player.transform.position);
        return BTNodeState.Running;
    }

    public override void Flip(Vector2 dir)
    {
        if (dir.x > 0f)
        {
            currentForm.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            currentForm.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    protected override bool CanAct()
    {
        if (!isActing && !isDie)
        {
            // 행동하기 전에 Walk 애니메이션 끄고 Flip 확인
            isWalking = false;
            Animator.SetBool("Walk", false);
            Flip(GameManager.Instance.Player.transform.position - currentForm.transform.position);

            return true;
        }
        else
        {
            return false;
        }
    }

    protected override bool IsInAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(currentForm.transform.position, GameManager.Instance.Player.transform.position);
        return distanceToPlayer <= data.attackDistance[0];
    }

    protected override bool IsPlayerInRoom()
    {
        return GameManager.Instance.roomManager.rooms[5].isPlayerInRoom;
    }

    private bool IsInDetectRange()
    {
        float distanceToPlayer = Vector2.Distance(currentForm.transform.position, GameManager.Instance.Player.transform.position);
        return distanceToPlayer <= data.detectRange;
    }

    private bool IsOutAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(currentForm.transform.position, GameManager.Instance.Player.transform.position);
        return (data.attackDistance[0] <= distanceToPlayer) && (distanceToPlayer <= data.detectRange);
    }

    private bool IsSlimeForm()
    {
        return !isTransformed;
    }

    private bool IsDemonForm()
    {
        return isTransformed;
    }

    private bool CanTransformation()
    {
        if (healthSystem.CurrentHealth <= 0f && !isTransformed)
        {
            isTransformed = true;
            return true;
        }
        return false;
    }

    public void StartTransformation()
    {
        if (skills != null)
        {
            skills.StopAllCoroutines();
        }

        demonForm.transform.position = slimeForm.transform.position;
        currentForm = demonForm;
        data = demonData;
        SetComponents();
        slimeForm.SetActive(false);
        demonForm.SetActive(true);

        skills = currentForm.GetComponentInChildren<DemonSlimeSkills>();
        skills.InitializeCooldowns();
        InitializeDemonFormBT();

        // 배경음
        SoundManager.Instance.PlayBGM(bossBgm);
        // 보스 UI
        healthSystem.OnHealthChanged += bossUI.UpdateBossHp;
        StatHandler demonFromStat = currentForm.GetComponentInChildren<StatHandler>();
        bossUI.ShowBossUI(demonFromStat);

    }

    protected override void OnDamage()
    {
        base.OnDamage();
    }

    protected override void OnDeath()
    {
        myRoom.portal.gameObject.SetActive(true);
        base.OnDeath();        
    }

    protected override void ResetBoss()
    {        
        if(isTransformed)
        {
            StartCoroutine(ResetBossCoroutine());        
        }
    }

    private IEnumerator ResetBossCoroutine()
    {
        
        bossUI.DisableBossUI();        
        // 일정 시간 대기 후 리셋
        yield return new WaitForSeconds(2.5f);
        
        if (skills != null)
        {
            skills.StopAllCoroutines();
        }

        currentForm = slimeForm;
        data = slimeData;

        SetComponents();
        slimeForm.SetActive(true);
        demonForm.SetActive(false);

        isTransformed = false;
        isDie = false;
        isActing = false;
        isWalking = false;
        Animator.SetBool("Walk", false);

        skills = currentForm.GetComponentInChildren<DemonSlimeSkills>();
        skills.InitializeCooldowns();

        InitializeBehaviorTree();
        root.Evaluate();

        soundManager.PlayBGM(bgmClip);
    }
}

