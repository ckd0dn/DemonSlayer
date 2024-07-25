using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenSlime : MonoBehaviour
{
    [Header("Stat Data")]
    public MonsterStatsSO data;

    [Header("King Slime Data")]
    public KingSlime kingSlime;

    [Header("Sound")]
    public AudioClip tentacleClip;

    [HideInInspector] public bool isDie = false;
    [HideInInspector] public bool isActing = false;
    [HideInInspector] public bool onPhase2 = false;
    [HideInInspector] public bool onPhase3 = false;

    [HideInInspector] public HealthSystem healthSystem;

    public Animator Animator { get; set; }
    private SpriteRenderer spriteRenderer;
    private QueenSlimeSkills skills;
    private PaintWhite paintWhite;
    private BTSelector root;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        skills = GetComponentInChildren<QueenSlimeSkills>();
        Animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        paintWhite = GetComponentInChildren<PaintWhite>();
    }

    private void Start()
    {
        healthSystem.OnDamage += OnDamage;        

        root = new BTSelector();

        BTSequence attackSequence = new BTSequence();
        BTSequence movementSequence = new BTSequence();
        BTSequence phaseChangeSequence = new BTSequence();
        BTSequence tentacleStingSequence = new BTSequence();

        BTCondition inAttackRange = new BTCondition(IsInAttackRange);
        BTCondition canActing = new BTCondition(CanAct);
        BTCondition canChangePhase = new BTCondition(CanChangePhase);

        BTAction tentacleSting = new BTAction(skills.TentacleStingAction);
        BTAction moveToTarget = new BTAction(MoveToTarget);
        BTAction changePhase = new BTAction(skills.SetPhaseAction);

        root.AddChild(phaseChangeSequence);
        {
            phaseChangeSequence.AddChild(canChangePhase);
            phaseChangeSequence.AddChild(changePhase);
        }
        root.AddChild(attackSequence);
        {            
            attackSequence.AddChild(tentacleStingSequence);
            {
                tentacleStingSequence.AddChild(canActing);
                tentacleStingSequence.AddChild(inAttackRange);
                tentacleStingSequence.AddChild(tentacleSting);
            }
        }
        root.AddChild(movementSequence);
        {
            movementSequence.AddChild(canActing);
            movementSequence.AddChild(moveToTarget);
        }

        root.Evaluate();
    }

    private bool CanChangePhase()
    {
        if (kingSlime.onPhase2 || kingSlime.onPhase3)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        root.Evaluate();        
    }   

    private bool IsInAttackRange()
    {
        if (GameManager.Instance.roomManager.rooms[6].isPlayerInRoom)
        {            
            float distanceToPlayer = Vector2.Distance(transform.position, kingSlime.transform.position);
            return distanceToPlayer <= data.attackRange;
        }
        return false;
    }

    private BTNodeState MoveToTarget()
    {
        Animator.SetBool("Walk", true);
        //transform.position = Vector2.MoveTowards(transform.position, kingSlime.transform.position, data.speed * Time.deltaTime);
        Flip(kingSlime.transform.position - transform.position);
        return BTNodeState.Running;
    }

    private void Flip(Vector2 dir)
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

    private bool CanAct()
    {
        return !isActing;
    }

    private void OnDamage()
    {
        //paintWhite.FlashWhite();
    }

    public void OnDeath()
    {
        isDie = true;
        isActing = true;
        Animator.SetTrigger("Death");
        Destroy(gameObject, 2f);
    }
}
