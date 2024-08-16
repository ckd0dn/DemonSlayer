using System.Collections;
using UnityEngine;

public class DemonSlimeSkills : MonoBehaviour
{
    private DemonSlime demonSlime;
    private HealthSystem playerHealth;
    private ObjectPool objectPool;

    [Header("Attack Data")]
    public BossSkillData meleeAttackData;
    public BossSkillData smashData;
    public BossSkillData chargeSmashData;
    public BossSkillData jumpSlamData;
    public BossSkillData breathData;
    public BossSkillData fireBallData;
    public float actionDelay = 0.5f;

    [Header("Attack Range")]
    public GameObject meleeAttackRange;
    public GameObject smashRange;
    public GameObject chargeSmashRange;
    public GameObject jumpSlamRange;
    public GameObject breathRange;
    private GameObject breathRangePivot;
    public Transform fireBallSpawnPoint;
    public Transform chargeSmashSpawnPoint;

    [Header("Effect Data")]
    public GameObject chargeSmashEffect;

    private WaitForSeconds actionDelayTime;

    public Coroutine currentCoroutine = null;

    private void Awake()
    {
        demonSlime = GetComponentInParent<DemonSlime>();
        objectPool = GetComponentInParent<ObjectPool>();
    }

    private void Start()
    {
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();

        actionDelayTime = new WaitForSeconds(actionDelay);

        breathRangePivot = breathRange.transform.parent.gameObject;

        InitializeCooldowns();
    }

    private void Update()
    {
        if (demonSlime.isTransformed)
        {
            UpdateCooldowns();
        }
    }

    public void InitializeCooldowns()
    {
        smashData.currentCooldown = smashData.cooldown;
        chargeSmashData.currentCooldown = chargeSmashData.cooldown;
        jumpSlamData.currentCooldown = jumpSlamData.cooldown;
        breathData.currentCooldown = breathData.cooldown;
        fireBallData.currentCooldown = fireBallData.cooldown;
    }

    private void UpdateCooldowns()
    {
        smashData.UpdateCooldown(Time.deltaTime);
        chargeSmashData.UpdateCooldown(Time.deltaTime);
        jumpSlamData.UpdateCooldown(Time.deltaTime);
        breathData.UpdateCooldown(Time.deltaTime);
        fireBallData.UpdateCooldown(Time.deltaTime);
    }

    #region SlimeForm

    #region MeleeAttack
    public BTNodeState MeleeAttackAction()
    {
        return SkillAction(meleeAttackData, "Smash");
    }

    public void MeleeAttack()
    {
        ExecuteCoroutine(MeleeAttackCoroutine());
    }

    public IEnumerator MeleeAttackCoroutine()
    {
        yield return PerformAttack(meleeAttackData, meleeAttackRange);
    }
    #endregion

    #region Transformation
    public BTNodeState TransformAction()
    {
        Transformation();
        return BTNodeState.Success;
    }

    public void Transformation()
    {
        ExecuteCoroutine(TransformCoroutine());
    }

    public IEnumerator TransformCoroutine()
    {
        demonSlime.isActing = true;
        demonSlime.healthSystem.isInvincibility = true;
        demonSlime.Animator.SetTrigger("Transformation");
               
        //yield return new WaitForSeconds(0.01f);
        //float animationRunTime = demonSlime.Animator.GetCurrentAnimatorClipInfo(0).Length;
        //yield return new WaitForSeconds(animationRunTime);
        yield return new WaitForSeconds(8.1f);
        
        demonSlime.StartTransformation();
        demonSlime.isActing = false;
        demonSlime.healthSystem.isInvincibility = false;
    }
    #endregion

    #endregion

    #region DemonForm

    #region Smash
    public BTNodeState SmashAction()
    {
        return SkillAction(smashData, "Smash");
    }

    public void Smash()
    {        
        ExecuteCoroutine(SmashCoroutine());
    }

    public IEnumerator SmashCoroutine()
    {
        yield return PerformAttack(smashData, smashRange);
    }
    #endregion

    #region ChargeSmash
    public BTNodeState ChargeSmashAction()
    {
        return SkillAction(chargeSmashData, "ChargeSmash");
    }

    public void ChargeSmash()
    {        
        ExecuteCoroutine(ChargeSmashCoroutine());
    }

    public IEnumerator ChargeSmashCoroutine()
    {
        chargeSmashEffect.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        GameObject chargeSmash = objectPool.SpawnFromPool("ChargeSmash");
        chargeSmash.transform.parent = demonSlime.currentForm.transform.parent; // Flip에 영향을 받지 않도록 부모 설정
        chargeSmash.transform.position = chargeSmashSpawnPoint.position;
        chargeSmash.SetActive(true);

        float directionX = demonSlime.currentForm.transform.rotation.eulerAngles.y == 0 ? -1 : 1; // 현재 바라보고 있는 방향, 왼쪽일 때 0
        Vector3 direction = new Vector3(directionX, 0).normalized;

        if (directionX < 0)
        {
            chargeSmash.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            chargeSmash.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        chargeSmash.GetComponent<Rigidbody2D>().velocity = direction * chargeSmashData.projectileSpeed;

        //SoundManager.Instance.PlaySFX();

        yield return actionDelayTime;
        chargeSmashEffect.SetActive(false);
        demonSlime.isActing = false;
    }
    #endregion

    #region JumpSlam
    public BTNodeState JumpSlamAction()
    {
        return SkillAction(jumpSlamData, "JumpSlam");
    }

    public void JumpSlam()
    {
        ExecuteCoroutine(JumpSlamCoroutine());
    }

    public IEnumerator JumpSlamCoroutine()
    {
        Vector3 playerPosition = GameManager.Instance.Player.transform.position;
        Vector3 targetDirection = (GameManager.Instance.Player.transform.position - demonSlime.transform.position).normalized;
        Vector3 targetPosition = new Vector3(playerPosition.x + targetDirection.x * 1.0f, demonSlime.fixedY, playerPosition.z);
            // GameManager.Instance.Player.transform.position + targetDirection * 2.0f + new Vector3(0, -1.5f, 0);

        float duration = 0.5f;
        float time = 0f;
                
        Vector3 startPosition = transform.parent.position + new Vector3(0, 1.5f, 0);        

        yield return new WaitForSeconds(0.2f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // t가 0.5일때 기준으로 각각 다른 이차함수 값을 구해 smoothT에 저장
            // 느리게 올라가고 빠르게 떨어지도록
            float smoothT = t < 0.5f ? (2 * t * t) : (-2 * (t * t) + (4 * t) - 1);

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);

            // 포물선을 그리기 위해서 선형 보간된 위치의 y값에 사인함수더해줌 0 ~ 파이까지
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * 3.0f;

            transform.parent.position = currentPosition;

            yield return null;
        }

        // 땅에 닿을 때 공격 실행
        yield return PerformAttack(jumpSlamData, jumpSlamRange);
    }
    #endregion

    #region Breath
    public BTNodeState BreathAction()
    {
        return SkillAction(breathData, "Breath");
    }

    public void Breath()
    {
        ExecuteCoroutine(BreathCoroutine());
    }

    public IEnumerator BreathCoroutine()
    {        
        float attackStartAngle = -45f;
        float attackEndAngle = 45f;
        float duration = 0.7f;
        float time = 0f;

        Quaternion originalRotation = breathRangePivot.transform.rotation;
        Quaternion attackStartRotation = Quaternion.Euler(0, 0, attackStartAngle);
        Quaternion attackEndRotation = Quaternion.Euler(0, 0, attackEndAngle);

        while (time < duration)
        {
            // 시간에 따라 피벗 회전
            time += Time.deltaTime;
            float t = time / duration;

            breathRangePivot.transform.localRotation = Quaternion.Lerp(attackStartRotation, attackEndRotation, t);

            // 공격 실행
            Collider2D[] hit = Physics2D.OverlapBoxAll(breathRange.transform.position, breathRange.transform.lossyScale, breathRange.transform.rotation.eulerAngles.z);
            if (hit != null)
            {
                foreach (Collider2D collider in hit)
                {
                    if (collider.CompareTag("Player") && !demonSlime.isDie)
                    {
                        playerHealth.ChangeHealth(-breathData.damage);
                    }
                }
            }
            yield return null;
        }
        breathRangePivot.transform.rotation = originalRotation;

        yield return actionDelayTime;
        yield return new WaitForSeconds(0.2f); // 애니메이션 시간으로 수정
        demonSlime.isActing = false;
    }
    #endregion

    #region FireBall
    // TODO :: 애니메이션 재생 전에 도망가도록 수정
    // 애니메이션 이벤트 쓰려면 노드를 추가해야할지도 모름
    public BTNodeState FireBallAction()
    {
        if (demonSlime.isActing || fireBallData.currentCooldown > 0f)
        {
            return BTNodeState.Failure;
        }
        demonSlime.isActing = true;

        FireBall();

        // StartCoroutine(RunAway());

        // demonSlime.Animator.SetBool("FireBall", true);
        fireBallData.currentCooldown = fireBallData.cooldown;
        return BTNodeState.Success;
        // return SkillAction(fireBallData, "FireBall");
    }

    public void FireBall()
    {
        ExecuteCoroutine(FireBallCoroutine());
    }

    public IEnumerator FireBallCoroutine()
    {
        // 거리 벌리기 
        float directionX = demonSlime.currentForm.transform.rotation.eulerAngles.y == 0 ? -1 : 1; // 현재 바라보고 있는 방향, 왼쪽일 때 0
        Vector3 direction = new Vector3(-directionX, 0).normalized;
        Vector3 target = demonSlime.currentForm.transform.position + direction * 13f;

        float time = 0f;
        float duration = 1f;

        demonSlime.Flip(direction); // 방향 전환
        demonSlime.Animator.SetBool("Walk", true);

        while (time < duration)
        {
            time += Time.deltaTime;
            demonSlime.currentForm.transform.position = Vector2.MoveTowards(demonSlime.currentForm.transform.position, target, demonSlime.data.speed * 2f * Time.deltaTime);           
            yield return null;
        }

        demonSlime.Animator.SetBool("Walk", false);

        demonSlime.Flip(-direction); // 다시 플레이어 바라보기
        yield return new WaitForSeconds(0.5f);

        // 공격 시작
        demonSlime.Animator.SetBool("FireBall", true);

        int projectileCount = 10;

        for (int i = 0; i < projectileCount; i++)
        {
            float startPositionRange = UnityEngine.Random.Range(-1f, 1f); // 생성 위치 범위
            float targetPositionRange = UnityEngine.Random.Range(-1.5f, 1.5f); // 발사 각도 조정

            GameObject fireBall = objectPool.SpawnFromPool("FireBall");
            fireBall.transform.parent = demonSlime.currentForm.transform.parent; // Flip에 영향을 받지 않도록 부모 설정
            fireBall.transform.position = fireBallSpawnPoint.position + new Vector3(0, startPositionRange);
            fireBall.SetActive(true);

            // 플레이어 중앙 + 랜덤 범위 설정
            Vector3 targetPosition = GameManager.Instance.Player.transform.position + new Vector3(0, -0.7f + targetPositionRange);
            Vector3 attackDirection = (targetPosition - fireBallSpawnPoint.position).normalized;

            fireBall.GetComponent<Rigidbody2D>().velocity = attackDirection * fireBallData.projectileSpeed;

            yield return new WaitForSeconds(fireBallData.delay);
        }
        //SoundManager.Instance.PlaySFX();

        yield return actionDelayTime;
        demonSlime.Animator.SetBool("FireBall", false);

        demonSlime.isActing = false;
    }
    #endregion

    #endregion

    // 코루틴 실행
    private void ExecuteCoroutine(IEnumerator coroutine)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(coroutine);
    }

    // 공격 상태 전환, 애니메이션 재생, 공격 코루틴 실행
    // 노드 실행 결과 반환
    private BTNodeState SkillAction(BossSkillData skillData, string animationTrigger)
    {
        if (demonSlime.isActing || skillData.currentCooldown > 0f)
        {
            return BTNodeState.Failure;
        }

        demonSlime.Animator.SetTrigger(animationTrigger);
        demonSlime.isActing = true;
        skillData.currentCooldown = skillData.cooldown;        
        return BTNodeState.Success;
    }

    // 일반적인 범위 공격
    private IEnumerator PerformAttack(BossSkillData skillData, GameObject range)
    {
        // TODO :: 이펙트도 SO안으로 넣기?
        // 이펙트 활성화
        //if (effect != null)
        //{
        //    effect.SetActive(true);
        //}
        // 공격 실행        
        //Collider2D[] hit = Physics2D.OverlapBoxAll(skillData.range.transform.position, skillData.range.transform.lossyScale, 0);
        Collider2D[] hit = Physics2D.OverlapBoxAll(range.transform.position, range.transform.lossyScale, 0);
        if (hit != null)
        {
            foreach (Collider2D collider in hit)
            {
                if (collider.CompareTag("Player") && !demonSlime.isDie)
                {
                    playerHealth.ChangeHealth(-skillData.damage);
                }
            }
        }
        // 사운드 재생
        //SoundManager.Instance.PlaySFX();

        yield return actionDelayTime;
        // 이펙트 비활성화
        //if (effect != null)
        //{
        //    effect.SetActive(false);
        //}

        demonSlime.isActing = false;
    }

    private void OnDrawGizmos()
    {
        DrawGizmo(smashRange.transform, Color.green);
        DrawGizmo(chargeSmashRange.transform, Color.magenta);
        DrawGizmo(jumpSlamRange.transform, Color.blue);
        DrawGizmo(breathRange.transform, Color.yellow);
    }

    private void DrawGizmo(Transform range, Color color)
    {
        Gizmos.color = color;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(range.position, range.rotation, range.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        //Gizmos.DrawWireCube(range.position, range.lossyScale);
    }
}
