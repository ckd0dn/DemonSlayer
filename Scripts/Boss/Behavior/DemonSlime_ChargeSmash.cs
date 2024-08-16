using UnityEngine;
using System.Collections;

public class DemonSlime_ChargeSmash : MonoBehaviour
{
    private DemonSlimeSkills demonSlimeData;
    private HealthSystem playerHealth;
    private Rigidbody2D rb;
    private Animator animator;

    private float time = 5f;
    private Coroutine TimerCoroutine;

    private void Start()
    {
        demonSlimeData = FindObjectOfType<DemonSlimeSkills>().GetComponent<DemonSlimeSkills>();        
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        TimerCoroutine = StartCoroutine(ActiveTimer());
    }

    private void OnDisable()
    {
        if (TimerCoroutine != null)
        {
            StopCoroutine(TimerCoroutine);
            TimerCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Instance.Player.stateMachine.IsRoll)
        {
            playerHealth.ChangeHealth(-demonSlimeData.chargeSmashData.damage);
            StartCoroutine(OnCollisionPlayer());
        }
    }

    private IEnumerator OnCollisionPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("OnCollision");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private IEnumerator ActiveTimer()
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeSelf)
        {
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
