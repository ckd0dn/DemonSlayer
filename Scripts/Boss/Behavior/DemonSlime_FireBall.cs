using UnityEngine;
using System.Collections;

public class DemonSlime_FireBall : MonoBehaviour
{
    private DemonSlimeSkills demonSlimeData;
    private HealthSystem playerHealth;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        demonSlimeData = FindObjectOfType<DemonSlimeSkills>().GetComponent<DemonSlimeSkills>();
        playerHealth = GameManager.Instance.Player.GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Instance.Player.stateMachine.IsRoll)
        {
            playerHealth.ChangeHealth(-demonSlimeData.fireBallData.damage);
            StartCoroutine(OnCollisionPlayer());
        }
    }

    private IEnumerator OnCollisionPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        animator.SetTrigger("OnCollision");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);        
        yield return new WaitForSeconds(0.9f);

        gameObject.SetActive(false);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
