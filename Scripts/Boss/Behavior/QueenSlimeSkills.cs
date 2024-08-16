using System.Collections;
using UnityEngine;

public class QueenSlimeSkills : MonoBehaviour
{
    private QueenSlime queenSlime;    
    private HealthSystem playerHealth;
    private ObjectPool objectPool;

    [Header("Attack Data")]
    public float poisonArrowDamage;
    public float poisonExplosionDamage;

    [Header("Skill Cooldown")]
    public float poisonExplosionCooldown = 10f;
    public float poisonArrowCooldown = 1.5f;

    [Header("Skill Data")]
    public GameObject poisonArrowPrefab;
    public GameObject poisonExplosionPrefab;
    public GameObject poisonExplosionRangePrefab;
    public float poisonArrowSpeed = 5f;
    public float poisonExplosionDelay = 0.5f;
    public float showRangeDuration = 1f;
    public float phaseChangeDuration = 3f;

    private float currentPoisonArrowCooldown;
    private float currentPoisonExplosionCooldown;

    private WaitForSeconds poisonExplosionDelayTime;
    private WaitForSeconds showRangeDurationTime;
    private WaitForSeconds phaseChangeDurationTime;

    private void Awake()
    {
        queenSlime = GetComponentInParent<QueenSlime>();
        objectPool = GetComponentInParent<ObjectPool>();
    }

    private void Start()
    {
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();

        currentPoisonArrowCooldown = poisonArrowCooldown;
        currentPoisonExplosionCooldown = poisonExplosionCooldown;

        poisonExplosionDelayTime = new WaitForSeconds(poisonExplosionDelay);
        showRangeDurationTime = new WaitForSeconds(showRangeDuration);
        phaseChangeDurationTime = new WaitForSeconds(phaseChangeDuration);
}

    private void Update()
    {
        if (GameManager.Instance.roomManager.rooms[6].isPlayerInRoom)
        {
            if (currentPoisonArrowCooldown > 0f)
            {
                currentPoisonArrowCooldown -= Time.deltaTime;
            }

            if (currentPoisonExplosionCooldown > 0f)
            {
                currentPoisonExplosionCooldown -= Time.deltaTime;
            }
        }
    }

    #region PoisonArrow
    public BTNodeState PoisonArrowAction()
    {
        if (currentPoisonArrowCooldown <= 0f)
        {
            PoisonArrowCoroutine();
            currentPoisonArrowCooldown = poisonArrowCooldown + Random.Range(-0.5f, 0.5f);

            return BTNodeState.Success;
        }
        else
        {
            return BTNodeState.Failure;
        }
    }

    public void PoisonArrowCoroutine()
    {
        StartCoroutine(PoisonArrow());
    }

    IEnumerator PoisonArrow()
    {
        queenSlime.isActing = true;

        GameObject poisonArrow = objectPool.SpawnFromPool("Bullet");        
        poisonArrow.transform.parent = gameObject.transform;
        poisonArrow.transform.position = queenSlime.transform.position;
        poisonArrow.SetActive(true);

        Vector3 direction = (GameManager.Instance.Player.transform.position - queenSlime.transform.position).normalized;
        poisonArrow.GetComponent<Rigidbody2D>().velocity = direction * poisonArrowSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 45f;
        poisonArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        SoundManager.Instance.PlaySFX(queenSlime.tentacleClip);

        if (!queenSlime.isOnPhaseChange)
        {
            queenSlime.isActing = false;
        }

        yield break;
    }
    #endregion

    #region PoisonExplosion
    public BTNodeState PoisonExplosionAction()
    {
        if (currentPoisonExplosionCooldown <= 0f)
        {
            PoisonExplosionCoroutine();            
            currentPoisonExplosionCooldown = poisonExplosionCooldown;

            return BTNodeState.Success;
        }
        else
        {
            return BTNodeState.Failure;
        }
    }

    public void PoisonExplosionCoroutine()
    {
        StartCoroutine(PoisonExplosion());
    }

    IEnumerator PoisonExplosion()
    {
        queenSlime.isActing = true;
        queenSlime.Animator.SetBool("Attack", true);

        int repeat = Random.Range(0, 4);

        for (int i = 0; i <= repeat; i++)
        {
            Vector3 playerPosition = GameManager.Instance.Player.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), 0f);
            Vector3 groundPosition = GetGroundPosition(playerPosition) + new Vector3(0f, 0.5f);
            Vector3 poisonExplosionPosition = groundPosition + new Vector3(0f, 0.5f);

            if (groundPosition != Vector3.zero)
            {
                GameObject poisonExplosionRange = Instantiate(poisonExplosionRangePrefab, groundPosition, Quaternion.identity);
                poisonExplosionRange.SetActive(true);

                yield return showRangeDurationTime;

                Destroy(poisonExplosionRange);

                GameObject poisonExplosion = Instantiate(poisonExplosionPrefab, poisonExplosionPosition, Quaternion.identity);
                poisonExplosion.transform.parent = queenSlime.transform;
                poisonExplosion.SetActive(true);
                SoundManager.Instance.PlaySFX(queenSlime.tentacleClip);

                Collider2D[] hit = Physics2D.OverlapBoxAll(poisonExplosion.transform.position, poisonExplosion.transform.lossyScale, 0);
                if (hit != null)
                {
                    foreach (Collider2D collider in hit)
                    {
                        if (collider.CompareTag("Player") && !queenSlime.isDie)
                        {
                            playerHealth.ChangeHealth(-poisonExplosionDamage);
                        }
                    }
                }
                Destroy(poisonExplosion, 0.5f);
            }
            yield return poisonExplosionDelayTime;
        }

        if (!queenSlime.isOnPhaseChange)
        {
            queenSlime.isActing = false;
        }
        queenSlime.Animator.SetBool("Attack", false);
    }

    private Vector3 GetGroundPosition(Vector3 playerPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(playerPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            return hit.point;
        }
        return Vector3.zero;
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
        queenSlime.isOnPhaseChange = true;

        queenSlime.Animator.SetBool("Heal", true);        

        yield return phaseChangeDurationTime;

        queenSlime.Animator.SetBool("Heal", false);

        queenSlime.isActing = false;
        queenSlime.isOnPhaseChange = false;
    }
    #endregion
}