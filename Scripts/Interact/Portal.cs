using UnityEngine;

public class Portal : InteractableObject
{
    Room teleportRoom;
    private Room myroom;
    public Transform teleportPosition;

    private void Awake()
    {
        myroom = GetComponentInParent<Room>();
    }

    protected override void Start()
    {
        base.Start();
        if(myroom.IsBossAlive)
        {
            gameObject.SetActive(false);
        }        
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
