using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemSO itemSO;

    [SerializeField] float chaseSpeed;
    [SerializeField] float maxTargetDistance = 10;

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ObtainItem()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        ChasePlayer();
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);

    private void ChasePlayer()
    {
        Vector3 targetPosition = GameManager.Instance.Player.transform.position;

        if (Vector3.Distance(targetPosition, transform.position) <= maxTargetDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
        }

    }

    public abstract void Drop(Vector3 postion);

}
