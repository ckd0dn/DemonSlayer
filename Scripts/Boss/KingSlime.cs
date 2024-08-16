using UnityEngine;

public class KingSlime : Boss
{
    [Header("Queen Slime Data")]
    public QueenSlime queenSlime;

    private KingSlimeSkills skills;

    [HideInInspector] public bool onPhase2 = false;
    [HideInInspector] public bool onPhase3 = false;

    [Header("Sound")]
    public AudioClip groundSlamClip;
    public AudioClip smashClip;

    protected override void Awake()
    {
        base.Awake();
        skills = GetComponentInChildren<KingSlimeSkills>();
    }

    protected override void Start()
    {
        base.Start();
        healthSystem.OnDeath += queenSlime.OnDeath;
        healthSystem.OnHealthChanged += bossUI.UpdateBossHp;
    }

    protected override void Update()
    {
        base.Update();        
    }

    #region BT Setting
    protected override void InitializeBehaviorTree()
    {
        root = new BTSelector();

        BTSequence phaseChangeSequence = CreatePhaseChangeSequence();
        BTSequence skillSequence = CreateSkillSequence();
        BTSequence meleeAttackSequence = CreateMeleeAttackSequence();
        BTSequence movementSequence = CreateMovementSequence();

        root.AddChild(phaseChangeSequence);
        root.AddChild(skillSequence);
        root.AddChild(meleeAttackSequence);
        root.AddChild(movementSequence);
    }

    private BTSequence CreatePhaseChangeSequence()
    {
        BTSequence phaseChangeSequence = new BTSequence();
        phaseChangeSequence.AddChild(new BTCondition(CanChangePhase));
        phaseChangeSequence.AddChild(new BTAction(skills.SetPhaseAction));
        return phaseChangeSequence;
    }

    private BTSequence CreateSkillSequence()
    {
        BTSequence skillSequence = new BTSequence();
        BTRandomSelector skillRandomSelector = new BTRandomSelector();

        skillRandomSelector.AddChild(new BTAction(skills.SmashAction));
        skillRandomSelector.AddChild(new BTAction(skills.GroundSlamAction));

        skillSequence.AddChild(new BTCondition(CanAct));
        skillSequence.AddChild(new BTCondition(IsInAttackRange));
        skillSequence.AddChild(skillRandomSelector);

        return skillSequence;
    }

    private BTSequence CreateMeleeAttackSequence()
    {
        BTSequence meleeAttackSequence = new BTSequence();
        meleeAttackSequence.AddChild(new BTCondition(CanAct));
        meleeAttackSequence.AddChild(new BTCondition(IsInAttackRange));
        meleeAttackSequence.AddChild(new BTAction(skills.MeleeAttackAction));

        return meleeAttackSequence;
    }

    private BTSequence CreateMovementSequence()
    {
        BTSequence movementSequence = new BTSequence();
        movementSequence.AddChild(new BTCondition(CanAct));
        movementSequence.AddChild(new BTAction(MoveToTarget));

        return movementSequence;
    }

    #endregion

    protected override BTNodeState MoveToTarget()
    {
        if (IsPlayerInRoom())
        {
            if (!IsInAttackRange())
            {
                Vector3 targetPosition = GameManager.Instance.Player.transform.position;
                targetPosition.y = fixedY;

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, data.speed * Time.deltaTime);
            }
            Flip(GameManager.Instance.Player.transform.position - transform.position);
            return BTNodeState.Running;
        }
        return BTNodeState.Failure;
    }

    public override void Flip(Vector2 dir)
    {
        if (dir.x < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    protected override bool CanAct()
    {
        return !isActing && !isDie;
    }

    protected override bool IsInAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        return distanceToPlayer <= data.detectRange;
    }

    protected override bool IsPlayerInRoom()
    {
        return GameManager.Instance.roomManager.rooms[6].isPlayerInRoom;
    }

    private bool CanChangePhase()
    {
        return (healthSystem.CurrentHealth / healthSystem.MaxHealth) * 100f <= 70f ? true : false;
    }

    protected override void OnDamage()
    {
        base.OnDamage();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        skills.groundSlamEffect.SetActive(false);
        skills.smashEffect.SetActive(false);
        skills.smashRange.SetActive(false);
    }

    protected override void ResetBoss()
    {
        onPhase2 = false;
        onPhase3 = false;
        isActing = false;
        isWalking = false;
        isInvincibility = false;
        skills.groundSlamEffect.SetActive(false);
        skills.smashEffect.SetActive(false);
        skills.smashRange.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 attackBoxSize = new Vector2(4f, 2f);
        float yOffset = 1f;

        Gizmos.color = Color.red;
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Vector2 startPosition = new Vector2(transform.position.x - 2f, transform.position.y + 1f);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? data.detectRange : -data.detectRange, yOffset);
        Vector2 attackOrigin = startPosition;
    }
}
