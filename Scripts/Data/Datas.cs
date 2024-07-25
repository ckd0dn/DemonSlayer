using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Datas
{
    public PlayerData playerData;
    public RoomManagerData roomManagerData;
    public float playTime;
    public bool isPlayIntro;
}

[System.Serializable]
public struct PlayerData
{
    public HealthSystemData healthSystemData;
    public int soulCount;
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
}

[System.Serializable]
public struct RoomData
{
    public bool isBossAlive; 
}

[System.Serializable]
public struct IntroData
{
    public bool[] isPlayIntros;
}
