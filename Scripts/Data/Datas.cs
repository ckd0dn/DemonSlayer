using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Datas
{
    public PlayerData playerData;
    public RoomManagerData roomManagerData;
    public CheckpointData[] checkpoints;
    public PLayerItemData playerItemData;
    public ItemData[] itemDatas;
    public float playTime;
    public bool isPlayIntro;
    public SkillData skillData;
}

[System.Serializable]
public struct PlayerData
{
    public HealthSystemData healthSystemData;
    public int soulCount;
    public bool DoubleJumpGet;
    public bool DashGet;
}

[System.Serializable]
public struct HealthSystemData
{
    private StatHandler statsHandler;

    public float CurrentHealth;
    public float CurrentMana;
    public float CurrentStamina;

    public float MaxHealth;
    public float MaxMana;
    public float MaxStamina;
}

[System.Serializable]
public struct RoomManagerData
{
    public RoomData[] rooms;
    public int currentRoomIdx;
    public Vector3 checkPointPosition;
    public string lastCheckPointName;
    public int lastCheckPointRoomIdx;
}

[System.Serializable]
public struct RoomData
{
    public bool isBossAlive;
    public bool isPlayerVisited;
}

[System.Serializable]
public struct CheckpointData
{
    public string checkPointName;
    public bool isDiscovered;
}

[System.Serializable]
public struct PLayerItemData
{
    public ItemSO[] equipItemListSO;
    public ItemSO[] invenItemListSO;
}

[System.Serializable]
public struct ItemData
{
    public ItemSO itemSO;
    public bool isGet;
}


[System.Serializable]
public struct SkillData
{
    public List<PlayerSkillSO> hasSkillListSO;
    public List<PlayerSkillSO> equipSkillListSO;
}

