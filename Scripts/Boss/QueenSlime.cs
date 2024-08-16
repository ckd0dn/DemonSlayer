using UnityEngine;

public class QueenSlime : Boss
{
    [Header("King Slime Data")]
    public KingSlime kingSlime;

    private QueenSlimeSkills skills;

    [HideInInspector] public bool onPhase2 = false;
    [HideInInspector] public bool onPhase3 = false;
    [HideInInspector] public bool isOnPhaseChange = false;

    [Header("Sound")]
    public AudioClip tentacleClip;

    protected override void Awake()
    {
        base.Awake();
        skills = GetComponentInChildren<QueenSlimeSkills>();
    }

    protected override void Start()
    {
        base.Start();
    }

    #region BT Setting
    protected override void InitializeBehaviorTree()
    {
        root = new BTSelector();

        var phaseChangeSequence = CreatePhaseChangeSequence();
        var poisonExplosionSequence = CreatePoisonExplosionSequence();
        var poisonArrowSequence = CreatePoisonArrowSequence();
        var movementSequence = CreateMovementSequence();

        root.AddChild(phaseChangeSequence);
        root.AddChild(poisonExplosionSequence);
        root.AddChild(poisonArrowSequence);
        root.AddChild(movementSequence);
    }

    private BTSequence CreatePhaseChangeSequence()
    {
        var phaseChangeSequence = new BTSequence();
        phaseChangeSequence.AddChild(new BTCondition(CanChangePhase));
        phaseChangeSequence.AddChild(new BTAction(skills.SetPhaseAction));
        return phaseChangeSequence;
    }

    private BTSequence CreatePoisonExplosionSequence()
    {
        var poisonExplosionSequence = new BTSequence();
        poisonExplosionSequence.AddChild(new BTCondition(CanAct));
        poisonExplosionSequence.AddChild(new BTCondition(IsInAttackRange));
        poisonExplosionSequence.AddChild(new BTAction(skills.PoisonExplosionAction));
        return poisonExplosionSequence;
    }

    private BTSequence CreatePoisonArrowSequence()
    {
        var poisonArrowSequence = new BTSequence();
        poisonArrowSequence.AddChild(new BTCondition(CanAct));
        poisonArrowSequence.AddChild(new BTCondition(IsInAttackRange));
        poisonArrowSequence.AddChild(new BTAction(skills.PoisonArrowAction));
        return poisonArrowSequence;
    }

    private BTSequence CreateMovementSequence()
    {
        var movementSequence = new BTSequence();
        movementSequence.AddChild(new BTCondition(CanAct));
        movementSequence.AddChild(new BTAction(MoveToTarget));
        return movementSequence;
    }
    #endregion

    protected override BTNodeState MoveToTarget()
    {
        Animator.SetBool("Walk", true);
        Flip(kingSlime.transform.position - transform.position);
        return BTNodeState.Running;
    }

    public override void Flip(Vector2 dir)
    {
        if (dir.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    protected override bool CanAct()
    {
        return !isActing && !isDie;
    }

    protected override bool IsInAttackRange()
    {
        if (GameManager.Instance.roomManager.rooms[6].isPlayerInRoom)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, kingSlime.transform.position);
            return distanceToPlayer <= data.attackDetectRange[0];
        }
        return false;
    }

    protected override bool IsPlayerInRoom()
    {
        return GameManager.Instance.roomManager.rooms[6].isPlayerInRoom;
    }

    protected override void ResetBoss()
    {
        onPhase2 = false;
        onPhase3 = false;
        isActing = false;
        isWalking = false;
        isInvincibility = false;
    }

    private bool CanChangePhase()
    {
        if (kingSlime.onPhase2 || kingSlime.onPhase3)
        {
            return true;
        }
        return false;
    }

    protected override void OnDamage()
    {
        base.OnDamage();
    }

    public void OnDeath()
    {
        Animator.SetTrigger("Death");
        isDie = true;
        isActing = true;
        Destroy(gameObject, 2f);
    }
}
