using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer; 
    public PlayerSO playerSO;

    public float attackRange = 1f;
    public Vector2 attackBoxSize = new Vector2(1f, 1f);
    public Vector2 thirdAttackBoxSize = new Vector2(2f, 1f);
    public float yOffset = 0f;
    public WaitForSeconds airAttackWait = new WaitForSeconds(0.15f);
    SoundManager soundManager;

    [SerializeField] private LayerMask layerMask;
    public float playerAtk;
    public float playerDamage;
    [SerializeField] private float firstAttackValue = 1;
    [SerializeField] private float secondAttackValue = 1.2f;
    [SerializeField] private float thirdAttackValue = 1.5f;

    [SerializeField] private float airAttackValue = 0.7f;

    public float getManaAmount = 5f;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        UpdatePlayerAtk();

        soundManager = SoundManager.Instance;
    }

    public void UpdatePlayerAtk()
    {
        playerAtk = GameManager.Instance.Player.StatHandler.CurrentStat.statsSO.damage;
        playerDamage = playerAtk;
    }

    public void ChangeAtk(float amount)
    {
        playerAtk = GameManager.Instance.Player.StatHandler.CurrentStat.statsSO.damage;

        if(amount > 0)
        {
            playerDamage = playerAtk * amount;
        }
    }

    public void PerformFirstAttack()
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -attackRange : attackRange, yOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, attackBoxSize, 0f, Vector2.zero, 0f, layerMask);

        SoundManager.Instance.PlaySFX(player.attackClip); 

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D enemy in hits)
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerDamage * firstAttackValue);
                    player.healthSystem.ChangeMana(getManaAmount);
                }
            }
        }
    }

    public void PerformSecondAttack()
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -attackRange : attackRange, yOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, attackBoxSize, 0f, Vector2.zero, 0f, layerMask);

        SoundManager.Instance.PlaySFX(player.attackClip);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D enemy in hits) 
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerDamage * secondAttackValue);
                    player.healthSystem.ChangeMana(getManaAmount);
                }
            }
        }
    }

    public void PerformThirdAttack()
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -attackRange : attackRange, yOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, thirdAttackBoxSize, 0f, Vector2.zero, 0f, layerMask);

        SoundManager.Instance.PlaySFX(player.thirdAttackClip); 

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D enemy in hits)
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerDamage * thirdAttackValue);
                    player.healthSystem.ChangeMana(getManaAmount);
                }
            }
        }
    }

    public IEnumerator PerformAirAttack()
    {
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y-1);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -attackRange : attackRange, yOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(attackOrigin, attackBoxSize, 0f, Vector2.zero, 0f, layerMask);

        SoundManager.Instance.PlaySFX(player.airAttackClip);
        yield return airAttackWait;
        SoundManager.Instance.PlaySFX(player.airAttackClip);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D enemy in hits)
            {
                HealthSystem enemyHealth = enemy.collider.gameObject.GetComponent<HealthSystem>();

                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-playerDamage * airAttackValue);
                    player.healthSystem.ChangeMana(getManaAmount);
                    yield return airAttackWait;
                    enemyHealth.ChangeHealth(-playerDamage * airAttackValue);
                    player.healthSystem.ChangeMana(getManaAmount);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 flipDirection = new Vector2(spriteRenderer.flipX ? -attackRange : attackRange, yOffset);
        Vector2 attackOrigin = startPosition + flipDirection;

        Gizmos.DrawWireCube(attackOrigin, attackBoxSize);
    }
}
