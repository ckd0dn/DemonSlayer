using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator anim;
    public HealthSystem playerHealth;
    public Rigidbody2D rb;
    public float projectileDestoryTime;
    public float projectileAnitime;
    public WaitForSeconds projectileAniDelayTimeWaitSeconds;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameManager.Instance.Player.healthSystem;
        projectileDestoryTime = 2f;
        projectileAnitime = 0.35f;
        projectileAniDelayTimeWaitSeconds = new WaitForSeconds(projectileAnitime);
    }
    private void OnEnable()
    {
        Invoke("Falseprojectile", projectileDestoryTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Falseprojectile()
    {
        StartCoroutine(BulletAnimation());
    }
    public IEnumerator BulletAnimation()
    {
        if (gameObject.activeSelf)
        {
            anim.SetBool(AnimationHashes.Destory, true);
            yield return projectileAniDelayTimeWaitSeconds;
            gameObject.SetActive(false);
        }
    }
}
