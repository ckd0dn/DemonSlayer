using System.IO;
using UnityEngine;

public enum ESaveFile
{
    save1,
    save2,
    save3,
}

public class DataManager : Singleton<DataManager>
{
    private string baseFilePath;
    // private string filePath;
    private const string save1 = "save1.txt";
    private const string save2 = "save2.txt";
    private const string save3 = "save3.txt";

    private string currentSaveFile = "";

    protected override void Awake()
    {
        base.Awake();
        baseFilePath = Application.persistentDataPath + "/";

        // 임시로 첫번쨰 파일
        currentSaveFile = save1;
    }

    public void SaveData()
    {
        Datas datas = new Datas();

        // 플레이어 데이터 저장
        datas.playerData = GameManager.Instance.Player.data;
        datas.playerData.healthSystemData = GameManager.Instance.Player.healthSystem.data;

        // 방 정보 저장
        datas.roomManagerData = GameManager.Instance.roomManager.data;

        // 보스 활성화 여부 저장
        for (int i = 0; i < GameManager.Instance.roomManager.rooms.Length; i++)
        {
            datas.roomManagerData.rooms[i] = GameManager.Instance.roomManager.rooms[i].data;

        }
        // 플레이 타임 저장
        datas.playTime = GameManager.Instance.timeManager.playTime;
        datas.isPlayIntro = true;

        // 체크포인트 데이터 저장
        CheckpointManager checkpointManager = GameManager.Instance.checkpointManager;
        datas.checkpoints = new CheckpointData[checkpointManager.checkpoints.Count];
        int index = 0;
        foreach (var checkpoint in checkpointManager.checkpoints.Values)
        {
            datas.checkpoints[index] = new CheckpointData
            {
                checkPointName = checkpoint.checkPointName,
                isDiscovered = checkpoint.isDiscovered,
            };
            index++;
        }

        // 아이템 저장
        RelicsItemSlot[] itemSlots = UIManager.Instance.mainMenuUI.invenItemRelics.relicsItemSlots;
        RelicsEquipSlot[] equipSlots = UIManager.Instance.mainMenuUI.invenEquipRelics.relicsEquipSlots;

        datas.playerItemData.invenItemListSO = new ItemSO[itemSlots.Length];
        datas.playerItemData.equipItemListSO = new ItemSO[equipSlots.Length];

        for (int i = 0; i < itemSlots.Length; i++)
        {
            RelicsItemSlot item = itemSlots[i];
            datas.playerItemData.invenItemListSO[i] = item.itemData;
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            RelicsEquipSlot item = equipSlots[i];
            datas.playerItemData.equipItemListSO[i] = item.EquipitemData;
        }

        // 스킬 저장
        datas.skillData.hasSkillListSO = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
        datas.skillData.equipSkillListSO = GameManager.Instance.Player.playerEquipSkill;

        // 필드 아이템 저장
        datas.itemDatas = new ItemData[GameManager.Instance.itemManager.items.Length];

        for (int i = 0; i < GameManager.Instance.itemManager.items.Length; i++)
        {
            datas.itemDatas[i].isGet = GameManager.Instance.itemManager.items[i].isGet;
            datas.itemDatas[i].itemSO = GameManager.Instance.itemManager.items[i].itemSO;
        }

        string json = JsonUtility.ToJson(datas);

        File.WriteAllText(baseFilePath + currentSaveFile, json);

        UIManager.Instance.ui.saveUI.Show();
    }


    public void LoadData()
    {

        if (File.Exists(baseFilePath + currentSaveFile))
        {
            // 불러올 데이터가 있다.

            string json = File.ReadAllText(baseFilePath + currentSaveFile);
            Datas datas = JsonUtility.FromJson<Datas>(json);

            // 플레이어 데이터 불러오기
            GameManager.Instance.Player.data = datas.playerData;
            GameManager.Instance.Player.healthSystem.data = datas.playerData.healthSystemData;

            // 방 정보 불러오기
            GameManager.Instance.roomManager.data = datas.roomManagerData;

            // 보스 활성화 여부 불러오기
            for (int i = 0; i < GameManager.Instance.roomManager.data.rooms.Length; i++)
            {
                GameManager.Instance.roomManager.rooms[i].data = datas.roomManagerData.rooms[i];
            }

            // 플레이 타임 불러오기
            GameManager.Instance.timeManager.loadTime = datas.playTime;

            // 체크포인트 데이터 불러오기
            CheckpointManager checkpointManager = GameManager.Instance.checkpointManager;
            foreach (var checkpointData in datas.checkpoints)
            {
                if (checkpointManager.checkpoints.TryGetValue(checkpointData.checkPointName, out CheckPoint checkpoint))
                {
                    checkpoint.isDiscovered = checkpointData.isDiscovered;
                }
            }

            // 마지막 체크포인트 지점을 플레이어 위치변경
            GameManager.Instance.Player.transform.position = GameManager.Instance.roomManager.checkPointPosition;

            // 아이템 불러오기
            ItemSO[] invenItemListSO = datas.playerItemData.invenItemListSO;
            ItemSO[] equipItemListSO = datas.playerItemData.equipItemListSO;

            for (int i = 0; i < invenItemListSO.Length; i++)
            {
                UIManager.Instance.mainMenuUI.AddItemToInventory(invenItemListSO[i]);
            }

            for (int i = 0; i < equipItemListSO.Length; i++)
            {
                UIManager.Instance.mainMenuUI.invenEquipRelics.AddEquipItemToSlot(equipItemListSO[i]);
                UIManager.Instance.mainMenuUI.invenEquipRelics.relicsEquipSlots[i].AddStat();
            }

            // 스킬 불러오기
            GameManager.Instance.Player.PlayerHasSkill.playerHasSkills = datas.skillData.hasSkillListSO;
            GameManager.Instance.Player.playerEquipSkill = datas.skillData.equipSkillListSO;

            // 필드 아이템 불러오기
            for (int i = 0; i < datas.itemDatas.Length; i++)
            {
                GameManager.Instance.itemManager.items[i].isGet = datas.itemDatas[i].isGet;
                GameManager.Instance.itemManager.items[i].itemSO = datas.itemDatas[i].itemSO;
            }
            // isGet이면 파괴
            GameManager.Instance.itemManager.DestroyItem();

            GameManager.Instance.Player.stateMachine.ChangeState(GameManager.Instance.Player.stateMachine.IdleState);
        }
    }

    public Datas[] LoadAllData()
    {
        Datas[] datas = new Datas[3];

        if (File.Exists(baseFilePath + save1))
        {
            string json = File.ReadAllText(baseFilePath + save1);
            datas[0] = JsonUtility.FromJson<Datas>(json);
        }
        if (File.Exists(baseFilePath + save2))
        {
            string json = File.ReadAllText(baseFilePath + save2);
            datas[1] = JsonUtility.FromJson<Datas>(json);
        }
        if (File.Exists(baseFilePath + save3))
        {
            string json = File.ReadAllText(baseFilePath + save3);
            datas[2] = JsonUtility.FromJson<Datas>(json);
        }

        return datas;
    }

    public void SelectSaveData(ESaveFile saveFile)
    {
        // 누른 버튼에 해당하는 세이브 파일을 현재 세이브로 

        switch (saveFile)
        {
            case ESaveFile.save1:
                currentSaveFile = save1;
                break;
            case ESaveFile.save2:
                currentSaveFile = save2;
                break;
            case ESaveFile.save3:
                currentSaveFile = save3;
                break;
            default:
                Debug.Log("게임 불러오기 오류");
                break;
        }
    }

    public void SelectClearData(ESaveFile saveFile)
    {
        switch (saveFile)
        {
            case ESaveFile.save1:
                currentSaveFile = save1;
                break;
            case ESaveFile.save2:
                currentSaveFile = save2;
                break;
            case ESaveFile.save3:
                currentSaveFile = save3;
                break;
            default:
                Debug.Log("error");
                break;
        }

        if (File.Exists(baseFilePath + currentSaveFile))
        {
            File.Delete(baseFilePath + currentSaveFile);
        }
    }

    public Datas GetData()
    {
        Datas datas = new Datas();

        if (File.Exists(baseFilePath + currentSaveFile))
        {
            // 불러올 데이터가 있다.

            string json = File.ReadAllText(baseFilePath + currentSaveFile);
            datas = JsonUtility.FromJson<Datas>(json);
        }

        return datas;
    }
}
