using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RoomManager
{
    public Room[] rooms;
    public int currentRoomIdx { get => data.currentRoomIdx; set => data.currentRoomIdx = value; }

    public Room currentRoom;
    public Vector3 checkPointPosition { get => data.checkPointPosition; set => data.checkPointPosition = value; }

    public int lastCheckPointRoomIdx { get => data.lastCheckPointRoomIdx; set => data.lastCheckPointRoomIdx = value; }
    public string lastCheckPointName { get => data.lastCheckPointName; set => data.lastCheckPointName = value; }

    public RoomManagerData data;

    public void Init()
    {
        currentRoomIdx = 0;

        GameObject[] roomObjs = GameObject.FindGameObjectsWithTag("Room");

        // roomObjs 배열을 이름을 기준으로 정렬
        //System.Array.Sort(roomObjs, (x, y) => x.name.CompareTo(y.name));
        Array.Sort(roomObjs, (x, y) =>
        {
            // 각 게임 오브젝트의 이름에서 숫자 추출
            int xNum = int.Parse(Regex.Match(x.name, @"\d+").Value);
            int yNum = int.Parse(Regex.Match(y.name, @"\d+").Value);

            // 숫자 기준으로 비교
            return xNum.CompareTo(yNum);
        });

        rooms = new Room[roomObjs.Length];

        for (int i = 0; i < roomObjs.Length; i++)
        {
            rooms[i] = roomObjs[i].GetComponent<Room>();
        }

        for (int i = 0; i < roomObjs.Length; i++)
        {
            rooms[i].roomIdx = i;
        }

        // data 초기화
        data.rooms = new RoomData[rooms.Length];

        for(int i = 0;i < data.rooms.Length; i++)
        {
            if (rooms[i].bossArray.Length > 0)
            {
                data.rooms[i].isBossAlive = true;
            }
            else
            {
                data.rooms[i].isBossAlive = false;
            }
        }

        InitRespawnCheckPoint();
    }


    // 리스폰될 체크포인트가 없는경우 첫번째 체크포인트로 초기화
    void InitRespawnCheckPoint()
    {
        for (int i = 0; i < rooms.Length; i++) 
        {
            if(rooms[i].checkPoint != null)
            {
                lastCheckPointRoomIdx = i;
                checkPointPosition = rooms[i].transform.position;
                break;
            }
        }
    }

}
