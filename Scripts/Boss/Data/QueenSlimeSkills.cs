using System;
using System.Collections;
using UnityEngine;

public class QueenSlimeSkills : MonoBehaviour
{
    private QueenSlime queenSlime;
    private HealthSystem healthSystem;
   
    private HealthSystem playerHealth;

    [Header("Attack Data")]
    public float tentacleStingDamage = 0;    
    public float phaseChangeDamage = 30;

    [Header("Skill Cooldown")]
    public float tentacleStingDelay = 0f;

    [Header("Skill Data")]    
    public GameObject tentaclePrefab;
    
    private WaitForSeconds tentacleStingDelayTime = new WaitForSeconds(0.7f);
    private WaitForSeconds phaseChangeTime = new WaitForSeconds(6.0f);

    private void Awake()
    {
        queenSlime = GetComponentInParent<QueenSlime>();
        healthSystem = GetComponentInParent<HealthSystem>();
    }

    private void Start()
    {
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();
    }

    private void Update()
    {
        tentacleStingDelay += Time.deltaTime;
    }

    #region Sting
    public BTNodeState TentacleStingAction()
    {
        if (tentacleStingDelay > 10f)
        {
            TentacleStingCoroutine();
            queenSlime.Animator.SetTrigger("Attack");
            tentacleStingDelay = 0f;

            return BTNodeState.Success;
        }
        else
        {
            return BTNodeState.Failure;
        }
    }

    public void TentacleStingCoroutine()
    {
        StartCoroutine(TentacleSting());
    }

    IEnumerator TentacleSting()
    {        
        queenSlime.isActing = true;
        for (int i = 0; i < 3; i++)
        {
            GameObject tentacle = Instantiate(tentaclePrefab);
            tentacle.transform.parent = queenSlime.transform;
            tentacle.SetActive(true);
            tentacle.transform.position = (GameManager.Instance.Player.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), -1.5f));
            SoundManager.Instance.PlaySFX(queenSlime.tentacleClip);
            yield return tentacleStingDelayTime;

            Collider2D[] hit = Physics2D.OverlapBoxAll(tentacle.transform.position, tentacle.transform.lossyScale, 0);
            if (hit != null)
            {
                foreach (Collider2D collider in hit)
                {
                    if (collider.CompareTag("Player") && !queenSlime.isDie)
                    {                        
                        playerHealth.ChangeHealth(-tentacleStingDamage);                     
                    }
                }
            }
            Destroy(tentacle, 0.5f);
        }
        queenSlime.isActing = false;
    }
    #endregion

    #region PhaseChange
    public BTNodeState SetPhaseAction()
    {        
        if (!queenSlime.onPhase2 && queenSlime.kingSlime.onPhase2)
        {
            queenSlime.onPhase2 = true;
            StartCoroutine(PhaseChanger());
            return BTNodeState.Success;
        }

        if (!queenSlime.onPhase3 && queenSlime.kingSlime.onPhase3)
        {
            queenSlime.onPhase3 = true;
            StartCoroutine(PhaseChanger());
            return BTNodeState.Success;
        }

        return BTNodeState.Failure;
    }

    IEnumerator PhaseChanger()
    {
        queenSlime.isActing = true;        

        queenSlime.Animator.SetBool("Heal", true);
        queenSlime.healthSystem.ChangeHealth(-phaseChangeDamage);

        yield return phaseChangeTime;

        queenSlime.Animator.SetBool("Heal", false);

        queenSlime.isActing = false;        
    }
    #endregion
}