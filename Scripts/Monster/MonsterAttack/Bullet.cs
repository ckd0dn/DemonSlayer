using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Animator anim;
    public HealthSystem playerHealth;
    private Vector2 BulletPostion;
    private Rigidbody2D rb;
    float bulletDamage;
    float bulletDestoryTime;
    float bulletAnitime;
    WaitForSeconds bulletAniDelayTimeWaitSeconds;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameManager.Instance.Player.healthSystem;
        bulletDamage = 10f;
        bulletDestoryTime = 2f;
        bulletAnitime = 0.35f;
        bulletAniDelayTimeWaitSeconds = new WaitForSeconds(bulletAnitime);
    }

    private void OnEnable()
    {
        Invoke("FalseBullet", bulletDestoryTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void SetBulletPosition(Vector2 direction)
    {
        BulletPostion = direction;
        rb.velocity = BulletPostion;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                if (playerHealth.CurrentHealth > 0)
                {
                    playerHealth.ChangeHealth(-bulletDamage);
                    StartCoroutine(BulletAnimation());
                }
            }
        }
    }

    private void FalseBullet()
    {
        StartCoroutine(BulletAnimation());
    }
    public IEnumerator BulletAnimation()
    {
        if (gameObject.activeSelf)
        {
            anim.SetBool(AnimationHashes.Destory, true);
            yield return bulletAniDelayTimeWaitSeconds;
            gameObject.SetActive(false);
        }
    }
}