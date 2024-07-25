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
    public bool DoubleJumpGet = false;
    public bool DashGet = false;
    public bool isEnemyDead = false;
    public bool firstSkillSlot = true;
    private float enemyDamage;
    [SerializeField] private float knockbackForce;
    private WaitForSeconds knockbackDelay = new WaitForSeconds(.5f);

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
    public PlayerSkill PlayerSkill { get; private set; }
    public PlayerHasSkill PlayerHasSkill { get; private set; }
    public StatHandler StatHandler { get; private set; }
    public PlayerSkillHandler PlayerSkillHandler { get; private set; }

    public PlayerStateMachine stateMachine;

    public PlayerData data;

    public int soulCount { get => data.soulCount; set => data.soulCount = value; }

    [Header("Sound")]
    public AudioClip attackClip;
    public AudioClip thirdAttackClip;
    public AudioClip airAttackClip;
    public AudioClip lightCutClip;
    public AudioClip holySlashClip;
    public AudioClip jumpClip;
    public AudioClip rollClip;
    public AudioClip hurtClip;


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
        StatHandler = GetComponent<StatHandler>();
        PlayerSkillHandler = GetComponentInChildren<PlayerSkillHandler>();

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
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();

        if (Rigidbody.velocity.y < 0)
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
            if (middleRayHit.collider != null && middleRayHit.distance > 0.5f)
            {
                isGrounded = true;
            }
            if (rightRayHit.collider != null && rightRayHit.distance > 0.5f)
            {
                isGrounded = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "monster" && !isEnemyDead)
        {
            enemyDamage = collision.gameObject.GetComponent<StatHandler>().CurrentStat.statsSO.damage;
            if (healthSystem.CurrentHealth > 0)
            {
                bool isChangeHealth = healthSystem.ChangeHealth(-enemyDamage);
                if(isChangeHealth)
                StartCoroutine(OnHitKnockback(collision.transform.position));
            }
        }
    }

    private IEnumerator OnHitKnockback(Vector2 targetPos)
    {
        int dirc = (transform.position.x - targetPos.x) > 0 ? 1: -1;
        Rigidbody.AddForce(new Vector2(dirc, 1) * knockbackForce, ForceMode2D.Impulse);
        yield return knockbackDelay;
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
        return  currentState.normalizedTime >= 1f;
        //currentState.IsName(animationName) &&
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
    }

    public void Respawn()
    {       
        GameManager.Instance.roomManager.lastCheckPointRoom.gameObject.SetActive(true);
        healthSystem.ResetHealth();
        this.transform.position = GameManager.Instance.roomManager.checkPointPosition;
        isRespawn = true;
    }
}