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
    RoomManager roomManager;

    private void Awake()
    {
        monsterObjectPool = GetComponentInParent<MonsterObjectPool>();
        checkpointRoom = GetComponentInParent<Room>();
        roomManager = GameManager.Instance.roomManager;
    }

    public override void Interact()
    {
        isDiscovered = true;

        UIManager.Instance.ToggleUI(ref checkPointUI, 1f, 1f, false, true);
        GameManager.Instance.TutorialCheckPoint = checkPointUI;
        SaveCheckPoint();
        GameManager.Instance.Player.healthSystem.ResetHealth();
    }

    public void SaveCheckPoint()
    {
        checkPointUI.currentCheckpoint = this;
        // 현재 체크포인트 이름
        checkPointUI.currentCheckpointName.text = checkPointName;
        // 인터렉트 글자 안보이게
        interactText.SetActive(false);
        // 마지막 체크포인트의 방저장
        GameManager.Instance.roomManager.lastCheckPointRoomIdx = GameManager.Instance.roomManager.currentRoom.roomIdx;
        // 현재 캐릭터가 있는 체크포인트의 위치, 이름 저장
        GameManager.Instance.roomManager.checkPointPosition = player.transform.position;
        GameManager.Instance.roomManager.lastCheckPointName = checkPointName;
        // 데이터 저장
        DataManager.Instance.SaveData();
        //체크포인트 저장 시 몬스터 활성화 
        SpawnMonster();
        //monsterObjectPool.SaveCheckpointMonsterSpawn();
    }

    public void SpawnMonster()
    {
        //보스방은 오브젝트 풀이 없으므로 -1 해줘야함 or if문에서 오브젝트 풀 null 체크하기 
        for (int i = 0; i < roomManager.rooms.Length; i++)
        {
            if (roomManager.rooms[i].monsterObjectPool != null)
            {
                roomManager.rooms[i].monsterObjectPool.SaveCheckpointMonsterSpawn();
            }
        }
    }
}
