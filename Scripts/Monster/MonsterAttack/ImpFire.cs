using UnityEngine;

public class ImpFire : MonoBehaviour
{
    public HealthSystem playerHealth;
    private float launchTime;
    float ImpFireDamage;
    float ImpFireDestoryTime;
    public Rigidbody2D rb;

    private void Awake()
    {
        ImpFireDamage = 15f;
        ImpFireDestoryTime = 3f;
        playerHealth = GameManager.Instance.Player.healthSystem;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Invoke("FalseImpFire", ImpFireDestoryTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    public void FireProjectile(Vector2 direction, Transform playertransform)
    {
        // 발사체의 초기 설정
        direction = (playertransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.velocity = direction * 5;
    }

    void FixedUpdate()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                if (playerHealth.CurrentHealth > 0)
                {
                    playerHealth.ChangeHealth(-ImpFireDamage);
                    gameObject.SetActive(false);
                }
            }
        }
    }
    private void FalseImpFire()
    {
        gameObject.SetActive(false);
    }
}
