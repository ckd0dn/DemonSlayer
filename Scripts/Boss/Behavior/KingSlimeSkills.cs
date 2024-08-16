using System.Collections;
using UnityEngine;

public class KingSlimeSkills : MonoBehaviour
{
    private KingSlime kingSlime;    
    private HealthSystem playerHealth;

    [Header("Attack Data")]
    public float meleeAttackDamage;
    public float smashDamage;
    public float groundSlamDamage;
    public float healOnPhaseChange;

    [Header("Skill Cooldown")]
    public float attackCooldown;
    public float smashCooldown;
    public float groundSlamCooldown;
    
    [Header("Skill Data")]
    public GameObject meleeAttackRange;
    public GameObject smashRange;
    public GameObject groundSlamRange;
    public float actionDelay = 1f;
    public float meleeAttackDelay = 0.3f;
    public float smashDelay = 0.6f;
    public float groundSlamDelay = 0.9f;
    public float phaseChangeDuration = 1f;

    [Header("Effect Data")]
    public GameObject groundSlamEffect;
    public GameObject smashEffect;
    public GameObject healingEffect;

    private float currentAttackCooldown;
    private float currentSmashCooldown;
    private float currentGroundSlamCooldown;

    private WaitForSeconds actionDelayTime;
    private WaitForSeconds meleeAttackDelayTime;
    private WaitForSeconds smashDelayTime;
    private WaitForSeconds groundSlamDelayTime;
    private WaitForSeconds phaseChangeDurationTime;

    private Coroutine currentCoroutine = null;

    private void Awake()
    {
        kingSlime = GetComponentInParent<KingSlime>();
    }

    private void Start()
    {
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();

        currentAttackCooldown = attackCooldown;
        currentSmashCooldown = smashCooldown;
        currentGroundSlamCooldown = groundSlamCooldown;

        actionDelayTime = new WaitForSeconds(actionDelay);
        meleeAttackDelayTime = new WaitForSeconds(meleeAttackDelay);
        smashDelayTime = new WaitForSeconds(smashDelay);
        groundSlamDelayTime = new WaitForSeconds(groundSlamDelay);
        phaseChangeDurationTime = new WaitForSeconds(phaseChangeDuration);
    }

    private void Update()
    {
        if (GameManager.Instance.roomManager.rooms[6].isPlayerInRoom)
        {
            if (currentAttackCooldown > 0f)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            if (currentSmashCooldown > 0f)
            {
                currentSmashCooldown -= Time.deltaTime;
            }

            if (currentGroundSlamCooldown > 0f)
            {
                currentGroundSlamCooldown -= Time.deltaTime;
            }
        }
    }

    #region MeleeAttack
    public BTNodeState MeleeAttackAction()
    {        
        if (kingSlime.isActing || kingSlime.isInvincibility)
        {
            return BTNodeState.Failure;
        }

        if (currentAttackCooldown <= 0f)
        {                    
            kingSlime.Animator.SetTrigger("MeleeAttack");
            currentAttackCooldown = attackCooldown;

            return BTNodeState.Success;
        }        
        else
        {
            return BTNodeState.Failure;
        }
    }

    public void MeleeAttack()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(MeleeAttackCoroutine());
    }

    private IEnumerator MeleeAttackCoroutine()
    {
        kingSlime.isActing = true;
        Collider2D[] hit = Physics2D.OverlapBoxAll(meleeAttackRange.transform.position, meleeAttackRange.transform.lossyScale, 0);
        if (hit != null)
        {
            foreach (Collider2D collider in hit)
            {
                if (collider.CompareTag("Player") && !kingSlime.isDie)
                {
                    playerHealth.ChangeHealth(-meleeAttackDamage);
                }
            }
        }
        yield return meleeAttackDelayTime;

        if (!kingSlime.isInvincibility)
        {
            kingSlime.isActing = false;
        }
    }
    #endregion

    #region Smash
    public BTNodeState SmashAction()
    {
        if (kingSlime.onPhase2 && !kingSlime.isActing && !kingSlime.isInvincibility)
        {
            if (currentSmashCooldown <= 0f)
            {
                kingSlime.Animator.SetTrigger("Smash");
                currentSmashCooldown = smashCooldown;
                return BTNodeState.Success;
            }            
        }
        return BTNodeState.Failure;
    }

    public void SmashCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Smash());        
    }

    IEnumerator Smash()
    {
        kingSlime.isActing = true;
        yield return smashDelayTime;
        
        Collider2D[] hit = Physics2D.OverlapBoxAll(smashRange.transform.position, smashRange.transform.lossyScale, 0);
        if (hit != null)
        {
            foreach(Collider2D collider in hit)
            {
                if (collider.CompareTag("Player") && !kingSlime.isDie)
                {
                    playerHealth.ChangeHealth(-smashDamage);                    
                }
            }            
        }
        smashEffect.SetActive(true);
        SoundManager.Instance.PlaySFX(kingSlime.smashClip);
        yield return actionDelayTime;
        smashEffect.SetActive(false);
        if (!kingSlime.isInvincibility)
        {
            kingSlime.isActing = false;
        }
    }
    #endregion

    #region Slam
    public BTNodeState GroundSlamAction()
    {
        if (kingSlime.onPhase3 && !kingSlime.isActing && !kingSlime.isInvincibility)
        {
            if (currentGroundSlamCooldown <= 0f)
            {                
                kingSlime.Animator.SetTrigger("GroundSlam");
                currentGroundSlamCooldown = groundSlamCooldown;
                return BTNodeState.Success;
            }
        }
        return BTNodeState.Failure;
    }
    public void GroundSlamCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(GroundSlam());
    }

    IEnumerator GroundSlam()
    {
        kingSlime.isActing = true;        
        yield return groundSlamDelayTime;

        Collider2D[] hit = Physics2D.OverlapBoxAll(groundSlamRange.transform.position, groundSlamRange.transform.lossyScale, 0);
        if (hit != null)
        {
            foreach (Collider2D collider in hit)
            {
                if (collider.CompareTag("Player") && !kingSlime.isDie)
                {
                    playerHealth.ChangeHealth(-groundSlamDamage);                    
                }
            }
        }        
        groundSlamEffect.SetActive(true);
        SoundManager.Instance.PlaySFX(kingSlime.groundSlamClip);        
        yield return actionDelayTime;
        groundSlamEffect.SetActive(false);
        if (!kingSlime.isInvincibility)
        {
            kingSlime.isActing = false;
        }
    }
    #endregion

    #region PhaseChange
    public BTNodeState SetPhaseAction()
    {
        if (kingSlime.isActing || kingSlime.isInvincibility)
        {
            return BTNodeState.Failure;
        }

        float healthPercentage = (kingSlime.healthSystem.CurrentHealth / kingSlime.healthSystem.MaxHealth) * 100f;

        if (30f < healthPercentage && healthPercentage <= 70f)
        {
            if (!kingSlime.onPhase2)
            {
                StopActingOnPhaseChange();
                kingSlime.onPhase2 = true;
                StartCoroutine(PhaseChanger());
                return BTNodeState.Success;               
            }
        }
        else if (healthPercentage <= 30)
        {
            if (!kingSlime.onPhase3)
            {
                StopActingOnPhaseChange();
                StartCoroutine(PhaseChanger());
                kingSlime.onPhase3 = true;
                return BTNodeState.Success;               
            }
        }
        return BTNodeState.Failure;
    }

    IEnumerator PhaseChanger()
    {
        kingSlime.isActing = true;
        kingSlime.isInvincibility = true;
        kingSlime.healthSystem.isInvincibility = true;
        healingEffect.SetActive(true);

        for (int i = 0; i < 3; i++)
        {         
            kingSlime.healthSystem.ChangeHealth(healOnPhaseChange);
            yield return phaseChangeDurationTime;
        }

        kingSlime.isActing = false;
        kingSlime.isInvincibility = false;
        kingSlime.healthSystem.isInvincibility = false;
        healingEffect.SetActive(false);
    }
    #endregion

    private void OnDrawGizmos()
    {        
        Gizmos.color = Color.green;        
        Gizmos.DrawWireCube(meleeAttackRange.transform.position, meleeAttackRange.transform.lossyScale);
        
        Gizmos.color = Color.red;        
        Gizmos.DrawWireCube(smashRange.transform.position, smashRange.transform.lossyScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundSlamRange.transform.position, groundSlamRange.transform.lossyScale);
    }

    private void StopActingOnPhaseChange()
    {
        // 코루틴 중지
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        kingSlime.isActing = false;
       
        // 이펙트 비활성화
        smashEffect.SetActive(false);
        groundSlamEffect.SetActive(false);
    }
}