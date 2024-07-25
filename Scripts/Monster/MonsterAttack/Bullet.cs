using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Animator anim;
    public HealthSystem playerHealth;
    private Vector2 BulletPostion;
    private Rigidbody2D rb;


    private void OnEnable()
    {
        Invoke("FalseBullet", 3f); 
    }

    private void OnDisable()
    {
        CancelInvoke(); 
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameManager.Instance.Player.healthSystem;
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
                    playerHealth.ChangeHealth(-10); 
                    anim.SetBool(AnimationHashes.Destory, true);
                    gameObject.SetActive(false);
                }
            }
            gameObject.SetActive(false); 
        }
    }

    private void OnBecameInvisible() // 카메라 밖에 나갔을 때 사라짐
    {
        gameObject.SetActive(false);
    }

    private void FalseBullet()
    {
        gameObject.SetActive(false); 
    }
}
