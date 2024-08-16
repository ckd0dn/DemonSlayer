using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength;
 

    public bool isRespawn = false;
    public bool isGrounded = true;
    public bool isJumped = false;
    public bool isSkillActive = false;
    public bool isinvincibility = false;
    public bool DoubleJumpGet { get => data.DoubleJumpGet; set => data.DoubleJumpGet = value; }
    public bool DashGet { get => data.DashGet; set => data.DashGet = value; }
    //public bool isEnemyDead = false;
    public bool firstSkillSlot = true;
    public bool preventFlipX = false;
    public float knockbackDelayTime = 1f;
    public float timeSinceLastKnockback = float.MaxValue;
    private float enemyDamage;
    [SerializeField] private float knockbackForce;
    [SerializeField] private WaitForSeconds knockbackDelay = new WaitForSeconds(1f);

    public List<PlayerSkillSO> playerEquipSkill = new List<PlayerSkillSO>();
    
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }   

    public HealthSystem healthSystem { get; set; }

    public Animator Animator { get; private set; }
    public PlayerController Input { get; private set; }
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerAttack PlayerAttack { get; private set; }
    public PlayerSkill PlayerSkill { get; private set; }
    public PlayerHasSkill PlayerHasSkill { get; private set; }
    public StatHandler StatHandler { get; private set; }
    public PlayerSkillHandler PlayerSkillHandler { get; private set; }

    public PlayerStateMachine stateMachine;

    public PlayerData data;

    public int soulCount { get => data.soulCount; set => data.soulCount = value; }

    [field: Header("Sound")]
    public AudioClip attackClip;
    public AudioClip thirdAttackClip;
    public AudioClip airAttackClip;
    public AudioClip lightCutClip;
    public AudioClip holySlashClip;
    public AudioClip jumpClip;
    public AudioClip rollClip;
    public AudioClip hurtClip;
    public AudioClip holyHealClip;
    public AudioClip saintHealClip;
    public AudioClip swordBuffClip;
    public AudioClip shieldBuffClip;
    public AudioClip bgmClip;
    public AudioClip dieClip;


    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        ForceReceiver = GetComponent<ForceReceiver>();
        healthSystem = GetComponent<HealthSystem>();
        PlayerSkill = GetComponentInChildren<PlayerSkill>();
        PlayerHasSkill = GetComponent<PlayerHasSkill>();
        PlayerSkillHandler = GetComponentInChildren<PlayerSkillHandler>();
        PlayerAttack = GetComponentInChildren<PlayerAttack>();
        StatHandler = GetComponent<StatHandler>();

        stateMachine = new PlayerStateMachine(this);
        groundLayer = LayerMask.GetMask("Ground");
        
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);

        healthSystem.OnDamage += OnHurt;
        healthSystem.OnDeath += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
        if (timeSinceLastKnockback < knockbackDelayTime)
        {
            timeSinceLastKnockback += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();

        if (Rigidbody.velocity.y < 1)
        {
            Vector2 leftRayOrigin = Rigidbody.position + Vector2.left * 0.3f;
            Vector2 middleRayOrigin = Rigidbody.position;
            Vector2 rightRayOrigin = Rigidbody.position + Vector2.right * 0.3f;

            Debug.DrawRay(leftRayOrigin, Vector2.down * 2f, Color.yellow);
            Debug.DrawRay(middleRayOrigin, Vector2.down * 2f, Color.yellow);
            Debug.DrawRay(rightRayOrigin, Vector2.down * 2f, Color.yellow);

            RaycastHit2D leftRayHit = Physics2D.Raycast(leftRayOrigin, Vector2.down, 2f, groundLayer);
            RaycastHit2D middleRayHit = Physics2D.Raycast(middleRayOrigin, Vector2.down, 2f, groundLayer);
            RaycastHit2D rightRayHit = Physics2D.Raycast(rightRayOrigin, Vector2.down, 2f, groundLayer);
            if (leftRayHit.collider != null && leftRayHit.distance > 0.5f)
            {
                isGrounded = true;
            }
            else if (middleRayHit.collider != null && middleRayHit.distance > 0.5f)
            {
                isGrounded = true;
            }
            else if (rightRayHit.collider != null && rightRayHit.distance > 0.5f)
            {
                isGrounded = true;
            }
            else isGrounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "bodyMonster")
        {
            enemyDamage = collision.gameObject.GetComponent<StatHandler>().CurrentStat.statsSO.damage;
            if (healthSystem.CurrentHealth > 0)
            {
                bool isChangeHealth = healthSystem.ChangeHealth(-enemyDamage);
                if(isChangeHealth && timeSinceLastKnockback >= knockbackDelayTime)
                StartCoroutine(OnHitKnockback(collision.transform.position));
            }
        }
    }

    private IEnumerator OnHitKnockback(Vector2 targetPos)
    {
        int dirc = (transform.position.x - targetPos.x) > 0 ? 1: -1;
        Rigidbody.AddForce(new Vector2(dirc, 1) * knockbackForce, ForceMode2D.Impulse);
        timeSinceLastKnockback = 0f;
        yield return knockbackDelay;
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
        //AnimatorTransitionInfo transitionInfo = Animator.GetAnimatorTransitionInfo(0);
        //bool isInTransition = transitionInfo.fullPathHash != 0;
        //bool isExitTimeReached = !isInTransition || transitionInfo.normalizedTime > 1.0f;
        return currentState.normalizedTime >= 1.0f;
        //currentState.IsName(animationName) &&
    }

    public bool IsAnimationFinishedWithName(string animationName)
    {
        AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(animationName) && currentState.normalizedTime >= 1.0f;

    }

    public void OnHurt()
    {
        if(!isSkillActive)          
        {
            stateMachine.ChangeState(stateMachine.HurtState);
            SoundManager.Instance.PlaySFX(hurtClip);
        }
    }

    void OnDie()
    {
        stateMachine.ChangeState(stateMachine.DeathState);

        if (UIManager.Instance.dieUI == null)
        UIManager.Instance.dieUI = UIManager.Instance.Show<DieUI>();
        UIManager.Instance.dieUI.fadeEffect.PlayerDie();
        SoundManager.Instance.WaitCurrentBgmAndPlayNext(dieClip,bgmClip);
    }

    public void Respawn()
    {
        int lastCheckPointRoomIdx = GameManager.Instance.roomManager.lastCheckPointRoomIdx;
        GameManager.Instance.roomManager.rooms[lastCheckPointRoomIdx].gameObject.SetActive(true);
        healthSystem.ResetHealth();
        this.transform.position = GameManager.Instance.roomManager.checkPointPosition;
        isRespawn = true;
    }

    public void FlipSprite(bool isLeft)
    {
        spriteRenderer.flipX = isLeft;
    }
}