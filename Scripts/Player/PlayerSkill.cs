using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public Player player;
    public Rigidbody2D _rigidbody;
    public SpriteRenderer spriteRenderer;
    public StatHandler statHandler;
    public PlayerSkillHandler playerSkillHandler;
    public HealthSystem healthSystem;
    public PlayerAttack playerAttack;
    
    public float holySlashRange = 1f;
    public float lightCutRange = 1f;
    public float holySlashYOffset = 3f;
    public float lightCutYOffset = 1f;
    public float lightCutMoveValue = 7f;
    public float shiledValue = 0.7f;
    
    public Vector2 holySlashBoxSize = new Vector2(1f, 1f);
    public Vector2 lightCutBoxSize = new Vector2(1f, 1f);
   
    public LayerMask layerMask;
    public LayerMask groundLayerMask;
    
    public bool isDashing = false;

    private WaitForSeconds DashDuration = new WaitForSeconds(0.2f);
    private WaitForSeconds SwordBuffDuration = new WaitForSeconds(15f);
    private WaitForSeconds SheildBuffDuration = new WaitForSeconds(10f);
    private WaitForSeconds lightCutWaitTime = new WaitForSeconds(.2f);
    private WaitForSeconds DashCooldown = new WaitForSeconds(1f);
    private WaitForSeconds lightCutDamageDelay = new WaitForSeconds(0.1f);
    private float DashForce;
    public float RollForce;
    private Vector2 tmpDir;

    void Start()
    {
        player = GetComponentInParent<Player>();
        _rigidbody = player.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        statHandler = GetComponentInParent<StatHandler>();
        playerSkillHandler = GetComponentInParent<PlayerSkillHandler>();
        healthSystem = GetComponentInParent<HealthSystem>();
        playerAttack = GetComponent<PlayerAttack>();
        DashForce = player.Data.GroundData.DashForce;
        RollForce = player.Data.GroundData.RollForce;
    }

    private void Update()
    {
        //Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        //Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -lightCutRange : lightCutRange, lightCutYOffset);
        //Vector2 groundCheckPosition = player.spriteRenderer.flipX ? new Vector2(-8, 0) : new Vector2(8, 0);
        //Vector2 groundCheckPosition2 = player.spriteRenderer.flipX ? new Vector2(1, 0) : new Vector2(-1, 0);

        //Vector2 attackOrigin = startPosition + flipDirection;
        //tmpDir = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;

        //float lightCutValue = lightCutMoveValue;
        //Debug.DrawRay(startPosition + groundCheckPosition2, tmpDir + groundCheckPosition, Color.red);
    }

    public IEnumerator LightCut()
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -lightCutRange : lightCutRange, lightCutYOffset);
        Vector2 groundCheckPosition = player.spriteRenderer.flipX ? new Vector2(1, 0) : new Vector2(-1, 0);
        Vector2 attackOrigin = startPosition + flipDirection;
        tmpDir = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;

        healthSystem.ChangeMana(-playerSkillHandler.LightCut.MPCost);

        float lightCutValue = lightCutMoveValue;

        // 벽에 부딪히는지 감지
        RaycastHit2D hit = Physics2D.Raycast(startPosition + groundCheckPosition, tmpDir, lightCutValue, groundLayerMask);
        Debug.DrawRay(startPosition, tmpDir, Color.red, lightCutValue);

        if (hit)
        {
            lightCutValue = lightCutMoveValue - (lightCutMoveValue - hit.distance);
        }

        Vector3 currentPosition = player.transform.position;
        float Force = (tmpDir == Vector2.right) ? lightCutValue : -lightCutValue;
        currentPosition.x += Force;
        player.transform.position = currentPosition;

        // 몬스터 감지
        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, lightCutBoxSize, 0f, Vector2.zero, 0f, layerMask);

        SoundManager.Instance.PlaySFX(player.lightCutClip); 

        if (hits.Length > 0)
        {
            yield return lightCutWaitTime;
            foreach (RaycastHit2D enemy in hits)
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();
                
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerAttack.playerDamage * playerSkillHandler.LightCut.DamageMultipleValue);
                    yield return lightCutDamageDelay;
                    enemyHealth.ChangeHealth(-playerAttack.playerDamage * playerSkillHandler.LightCut.DamageMultipleValue);
                    yield return lightCutDamageDelay;
                    enemyHealth.ChangeHealth(-playerAttack.playerDamage * playerSkillHandler.LightCut.DamageMultipleValue);
                }
            }
        }
    }

    public void HolySlash()
    {
        healthSystem.ChangeMana(-playerSkillHandler.HolySlash.MPCost);

        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -holySlashRange : holySlashRange, holySlashYOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, holySlashBoxSize, 0f, Vector2.zero, 0f, layerMask);      

        SoundManager.Instance.PlaySFX(player.holySlashClip);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D enemy in hits)
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerAttack.playerDamage * playerSkillHandler.HolySlash.DamageMultipleValue);
                }
            }
        }
    }

    public void HolyHeal()
    {
        healthSystem.ChangeMana(-playerSkillHandler.HolyHeal.MPCost);

        player.healthSystem.ChangeHealth(playerSkillHandler.HolyHeal.HealAmount);
        SoundManager.Instance.PlaySFX(player.holyHealClip);
    }

    public void SaintHeal()
    {
        healthSystem.ChangeMana(-playerSkillHandler.SaintHeal.MPCost);

        player.healthSystem.ChangeHealth(playerSkillHandler.SaintHeal.HealAmount);
        SoundManager.Instance.PlaySFX(player.saintHealClip);

    }

    public IEnumerator SwordBuff()
    {
        healthSystem.ChangeMana(-playerSkillHandler.SwordBuff.MPCost);

        float currentPlayerDamage = playerAttack.playerDamage;

        SoundManager.Instance.PlaySFX(player.swordBuffClip);

        playerAttack.ChangeAtk(playerSkillHandler.SwordBuff.BuffAmount);
        UIManager.Instance.buffIcon.swordBuffIcon.enabled = true;
        yield return SwordBuffDuration;
        UIManager.Instance.buffIcon.swordBuffIcon.enabled = false;
        playerAttack.playerDamage = currentPlayerDamage;
    }

    public IEnumerator ShieldBuff()
    {
       
        healthSystem.ChangeMana(-playerSkillHandler.ShieldBuff.MPCost);

        SoundManager.Instance.PlaySFX(player.shieldBuffClip);

        healthSystem.isShieldBuff = true;
        UIManager.Instance.buffIcon.shieldBuffIcon.enabled = true;
        yield return SheildBuffDuration;
        UIManager.Instance.buffIcon.shieldBuffIcon.enabled = false;
        healthSystem.isShieldBuff = false;

    }

    public void RollStart()
    {
        Vector2 vec2 = Vector2.left;
        if(player.stateMachine.MovementInput.x == 0)
        {
            vec2 = new Vector2(player.spriteRenderer.flipX ? -1 : 1, 0f);
        }
        else
        {
            vec2 = player.stateMachine.MovementInput.normalized;
        }
        _rigidbody.velocity = vec2 * RollForce;
        if (!GameManager.Instance.Player.stateMachine.IsRoll)
        {
            SoundManager.Instance.PlaySFX(GameManager.Instance.Player.rollClip);
        }
    }

    public void DashStart()
    {
        if(player.Rigidbody.gravityScale != 7.0f)
        {
            player.Rigidbody.gravityScale = 7f;
        }
        var originalGravity = player.Rigidbody.gravityScale;
        Vector2 vec2 = Vector2.left;
        if (player.stateMachine.MovementInput.x == 0)
        {
            vec2 = new Vector2(player.spriteRenderer.flipX ? -1 : 1, 0f);
        }
        else
        {
            vec2 = player.stateMachine.MovementInput.normalized;
            player.FlipSprite(vec2.x < 0);
        }
        _rigidbody.gravityScale = originalGravity * 0.1f;
        _rigidbody.velocity = vec2 * DashForce;
        //_rigidbody.velocity = new Vector2(player.transform.localScale.x * DashForce, 0);
        isDashing = true;
        StartCoroutine(ResetGravity(originalGravity));
    }

    IEnumerator ResetGravity(float originalGravity)
    {

        // 0.2�� ���� �뽬 ȿ�� ���� (�ʿ��� �ð� ���� ����)
        yield return DashDuration;
        // �߷��� ���� ������ �ǵ���

        _rigidbody.gravityScale = originalGravity;
        //_rigidbody.velocity = new Vector2(0, 0);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -holySlashRange : holySlashRange, holySlashYOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        Gizmos.DrawWireCube(attackOrigin, holySlashBoxSize);

        Gizmos.color = Color.yellow;

        Vector2 lightCutstartPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 lightCutflipDirection = new Vector2(spriteRenderer.flipX ? -lightCutRange : lightCutRange, lightCutYOffset);
        Vector2 lightCutattackOrigin = lightCutstartPosition + lightCutflipDirection;

        Gizmos.DrawWireCube(lightCutattackOrigin, lightCutBoxSize);
    }
}
