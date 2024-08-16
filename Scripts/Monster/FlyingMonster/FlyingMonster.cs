using System.Collections;
using UnityEngine;

public class FlyingMonster : Monster
{
    public BoxCollider2D boxCollider2D;
    public bool Cancol;
    public Transform Room;
    public bool ChasingDelay;

    protected override void Awake()
    {
        base.Awake();
        boxCollider2D = GetComponent<BoxCollider2D>();
        Cancol = true;
        Room = transform.parent;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        boxCollider2D.enabled = true;
    }


    public void OnTriggerExit2D(Collider2D collider)
    {
        if (gameObject.activeSelf && Cancol && Room.gameObject.activeInHierarchy)
        {
            if (collider.tag == "Room")
            {
                StartCoroutine(Collisiondelay());
                Cancol = false;
                if (isRight)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    isRight = false;
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                    isRight = true;
                }
            }
        }
    }

    public IEnumerator Collisiondelay()
    {
        boxCollider2D.enabled = false;
        ChasingDelay = true;
        yield return new WaitForSeconds(2f);
        boxCollider2D.enabled = true;
        ChasingDelay = false;
        Cancol = true;
    }
}
