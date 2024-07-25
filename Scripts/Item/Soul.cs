using TMPro;
using UnityEngine;

public class Soul : Item
{
    Player player;
    SoulUI soulUI;

    [SerializeField] protected float explosionForce; // 퍼지는 힘의 크기
    [SerializeField] protected float upForce; // 초기 위로 가는 힘

    [SerializeField] private AudioClip obtainClip;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ObtainItem();
            ObtainSoul();
        }
    }
    
    void Start()
    {
        soulUI = UIManager.Instance.soulUI;
        player = GameManager.Instance.Player;
    }
    public void ObtainSoul()
    {
        player.soulCount++;
        soulUI.UpdateSoulUI();
        SoundManager.Instance.PlaySFX(obtainClip);
    }

    public override void Drop(Vector3 postion)
    {
        float randomDirX = Random.Range(-1f, 1f);
        float dirY = 1f;
        Vector2 dropDir = new Vector2(randomDirX, dirY).normalized;
        transform.position = new Vector3(postion.x, postion.y + 0.5f, postion.z);
        transform.rotation = Quaternion.identity;

        Vector2 force = dropDir * explosionForce + Vector2.up * upForce;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

}
