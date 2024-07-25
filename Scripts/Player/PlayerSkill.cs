using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerSkill : MonoBehaviour
{
    public Player player;
    public Rigidbody2D _rigidbody;
    public SpriteRenderer spriteRenderer;
    public StatHandler statHandler;
    public PlayerSkillHandler playerSkillHandler;
    public HealthSystem healthSystem;
    
    public float holySlashRange = 1f;
    public float lightCutRange = 1f;
    public float holySlashYOffset = 3f;
    public float lightCutYOffset = 1f;
    
    public Vector2 holySlashBoxSize = new Vector2(1f, 1f);
    public Vector2 lightCutBoxSize = new Vector2(1f, 1f);
   
    public LayerMask layerMask;
    
    public bool isDashing = false;

    private WaitForSeconds SwordBuffDuration = new WaitForSeconds(15f);
    private WaitForSeconds lightCutWaitTime = new WaitForSeconds(.2f);
    private WaitForSeconds DashCooldown = new WaitForSeconds(1f);
    private float playerDamage;
    private float DashForce;
    private Vector2 tmpDir;

    void Start()
    {
        player = GetComponentInParent<Player>();
        _rigidbody = player.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        statHandler = GetComponentInParent<StatHandler>();
        playerSkillHandler = GetComponentInParent<PlayerSkillHandler>();
        healthSystem = GetComponentInParent<HealthSystem>();
        DashForce = player.Data.GroundData.DashForce;

        playerDamage = statHandler.CurrentStat.statsSO.damage;
    }

    public IEnumerator LightCut()
    {
        healthSystem.ChangeMana(-playerSkillHandler.LightCut.MPCost);
        tmpDir = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        //player.Rigidbody.AddForce((tmpDir) * lightCutForce, ForceMode2D.Impulse);
        Vector3 currentPosition = player.transform.position;
        int Force = (tmpDir == Vector2.right) ? 5 : -5;
        currentPosition.x += Force;
        player.transform.position = currentPosition;

        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -lightCutRange : lightCutRange, lightCutYOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

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
                    enemyHealth.ChangeHealth(-playerDamage * playerSkillHandler.LightCut.DamageMultipleValue);
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
                    enemyHealth.ChangeHealth(-playerDamage * playerSkillHandler.HolySlash.DamageMultipleValue);
                }
            }
        }
    }

    public void HolyHeal()
    {
        healthSystem.ChangeMana(-playerSkillHandler.HolyHeal.MPCost);

        player.healthSystem.ChangeHealth(playerSkillHandler.HolyHeal.HealAmount);
    }

    public void SaintHeal()
    {
        healthSystem.ChangeMana(-playerSkillHandler.SaintHeal.MPCost);

        player.healthSystem.ChangeHealth(playerSkillHandler.SaintHeal.HealAmount);
    }

    public IEnumerator SwordBuff()
    {
        healthSystem.ChangeMana(-playerSkillHandler.SwordBuff.MPCost);

        playerDamage += (int)playerSkillHandler.SwordBuff.BuffAmount;
        yield return SwordBuffDuration;
        playerDamage -= (int)playerSkillHandler.SwordBuff.BuffAmount;
    }

    public void ShieldBuff()
    {
        healthSystem.ChangeMana(-playerSkillHandler.ShieldBuff.MPCost);

    }

    public void DashStart()
    {
        isDashing = true;
        var originalGravity = player.Rigidbody.gravityScale;
        Vector2 tmpDir = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;
        _rigidbody.gravityScale = 0f;
        _rigidbody.velocity = new Vector2(tmpDir.x * DashForce, _rigidbody.velocity.y);
        //_rigidbody.velocity = new Vector2(player.transform.localScale.x * DashForce, 0);

        StartCoroutine(ResetGravity(originalGravity));

        isDashing = false;
    }

    IEnumerator ResetGravity(float originalGravity)
    {

        // 0.2초 동안 대쉬 효과 유지 (필요한 시간 조정 가능)
        yield return new WaitForSeconds(0.2f);
        // 중력을 원래 값으로 되돌림

        _rigidbody.gravityScale = originalGravity;
        //_rigidbody.velocity = new Vector2(0, 0);

    }

public void DashStop()
    { 
        
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
