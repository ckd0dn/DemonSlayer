using UnityEngine;

public class Portal : InteractableObject
{
    Room teleportRoom; 
    public Transform teleportPosition;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
        teleportRoom = teleportPosition.GetComponentInParent<Room>();
    }

    public override void Interact()
    {
        teleportRoom.gameObject.SetActive(true);
        Teleport();
    }

    public void Teleport()
    {
        GameManager.Instance.Player.transform.position = teleportPosition.position;
    }
}
