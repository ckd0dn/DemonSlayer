using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeSkills : MonoBehaviour
{
    private KingSlime kingSlime;    
    //private Rigidbody2D player;
    private HealthSystem playerHealth;

    // TODO :: 데이터 SO화, 쿨타임 & 대미지
    [Header("Attack Data")]
    public float meleeAttackDamage = 5;
    public float smashDamage = 10;
    public float groundSlamDamage = 20;
    public float healOnPhaseChange = 10;

    [Header("Skill Cooldown")]
    public float attackDelay = 0f;
    public float smashDelay = 0f;
    public float groundSlamDelay = 0f;  
    
    [Header("Skill Data")]
    public GameObject meleeAttackRange;
    public GameObject smashRange;
    public GameObject groundSlamRange;

    [Header("Effect Data")]
    public GameObject groundSlamEffect;
    public GameObject smashEffect;
    public GameObject healingEffect;

    private WaitForSeconds actionDelayTime = new WaitForSeconds(1.0f);
    private WaitForSeconds smashDelayTime = new WaitForSeconds(1.0f);
    private WaitForSeconds slamDelayTime = new WaitForSeconds(1.1f);
    private WaitForSeconds phaseDelayTime = new WaitForSeconds(2.0f);

    private void Awake()
    {
        kingSlime = GetComponentInParent<KingSlime>();
    }

    private void Start()
    {
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();
    }

    private void Update()
    {
        attackDelay += Time.deltaTime;
        smashDelay += Time.deltaTime;
        groundSlamDelay += Time.deltaTime;
    }

    #region MeleeAttack
    public BTNodeState MeleeAttackAction()
    {        
        if (kingSlime.isActing)
        {
            return BTNodeState.Failure;
        }

        if (attackDelay > 3f)
        {                    
            kingSlime.Animator.SetTrigger("MeleeAttack");
            attackDelay = 0f;

            return BTNodeState.Success;
        }        
        else
        {
            return BTNodeState.Failure;
        }
    }

    public void MeleeAttack()
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
        kingSlime.isActing = false;
    }
    #endregion

    #region Smash
    public BTNodeState SmashAction()
    {
        if (kingSlime.onPhase2 && !kingSlime.isActing)
        {
            if (smashDelay > 5f)
            {
                kingSlime.isActing = true;
                kingSlime.Animator.SetTrigger("Smash");
                smashDelay = 0f;

                return BTNodeState.Success;
            }            
        }
        return BTNodeState.Failure;
    }

    public void SmashCoroutine()
    {       
        StartCoroutine(Smash());        
    }

    IEnumerator Smash()
    {
        kingSlime.isActing = true;
        //smashRange.SetActive(true);
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
        //smashRange.SetActive(false);
        smashEffect.SetActive(true);
        SoundManager.Instance.PlaySFX(kingSlime.smashClip);
        yield return actionDelayTime;
        smashEffect.SetActive(false);
        kingSlime.isActing = false;
    }
    #endregion

    #region Slam
    public BTNodeState GroundSlamAction()
    {
        if (kingSlime.onPhase3 && !kingSlime.isActing)
        {
            if (groundSlamDelay > 7f)
            {                
                kingSlime.Animator.SetTrigger("GroundSlam");
                groundSlamDelay = 0f;

                return BTNodeState.Success;
            }
        }
        return BTNodeState.Failure;
    }
    public void GroundSlamCoroutine()
    {
        StartCoroutine(GroundSlam());
    }

    IEnumerator GroundSlam()
    {
        kingSlime.isActing = true;        
        yield return slamDelayTime;

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
        kingSlime.isActing = false;
    }
    #endregion

    #region PhaseChange
    public BTNodeState SetPhaseAction()
    {
        if (30f < (kingSlime.healthSystem.CurrentHealth / kingSlime.healthSystem.MaxHealth * 100f) && (kingSlime.healthSystem.CurrentHealth / kingSlime.healthSystem.MaxHealth * 100f) <= 70f)
        {
            if (kingSlime.onPhase2 == false)
            {
                StopActingOnPhaseChange();
                StartCoroutine(PhaseChanger());
                kingSlime.onPhase2 = true;
                // 2페이즈 변경점 추가
            }
            else
            {
                return BTNodeState.Failure;
            }
        }
        else if ((kingSlime.healthSystem.CurrentHealth / kingSlime.healthSystem.MaxHealth * 100f) <= 30)
        {
            if (kingSlime.onPhase3 == false)
            {
                StopActingOnPhaseChange();
                StartCoroutine(PhaseChanger());
                kingSlime.onPhase3 = true;
                // 3페이즈 변경점 추가
            }
            else
            {
                return BTNodeState.Failure;
            }
        }
        
        return BTNodeState.Success;
    }

    IEnumerator PhaseChanger()
    {
        kingSlime.isActing = true;
        kingSlime.healthSystem.isInvincibility = true;
        healingEffect.SetActive(true);

        for (int i = 0; i < 3; i++)
        {         
            kingSlime.healthSystem.ChangeHealth(healOnPhaseChange);
            yield return phaseDelayTime;
        }

        kingSlime.isActing = false;
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
        StopCoroutine(Smash());
        StopCoroutine(GroundSlam());

        // 이펙트 비활성화
        smashEffect.SetActive(false);
        groundSlamEffect.SetActive(false);
    }
}