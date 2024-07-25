using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : InteractableObject
{
    CheckPointUI checkPointUI;
    public string checkPointName;
    public bool isDiscovered = false;
    public Room checkpointRoom;
    public GameObject checkpointIcon;

    MonsterObjectPool monsterObjectPool;

    private void Awake()
    {
        monsterObjectPool = GetComponentInParent<MonsterObjectPool>();
    }    

    public override void Interact()
    {
        isDiscovered = true;
        checkpointRoom = GetComponentInParent<Room>();

        SaveCheckPoint();
        GameManager.Instance.Player.healthSystem.ResetHealth();
        UIManager.Instance.ToggleUI(ref checkPointUI, 1f, 1f, false, true);
    }

    public void SaveCheckPoint()
    {
        // 마지막 체크포인트의 방저장
        GameManager.Instance.roomManager.lastCheckPointRoom = GameManager.Instance.roomManager.currentRoom;
        // 현재 캐릭터가 있는 체크포인트의 위치, 이름 저장
        GameManager.Instance.roomManager.checkPointPosition = player.transform.position;
        GameManager.Instance.roomManager.lastCheckPointName = checkPointName;
        // 데이터 저장
        DataManager.Instance.SaveData();
        monsterObjectPool.SaveCheckpoint();
    }

}
