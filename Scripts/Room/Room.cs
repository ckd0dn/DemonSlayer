using Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room")]
    public int roomIdx;
    public Room[] movableRoom;
    public bool isPlayerInRoom = false;

    private PolygonCollider2D camCollider2D;
    private CinemachineConfiner2D cinemachineConfiner2D;

    [Header("CheckPoint")]
    public CheckPoint checkPoint;

    [Header("Boss")]
    public bool isBossRoom = false;
    public bool IsBossAlive { get => data.isBossAlive; set => data.isBossAlive = value; }
    public GameObject[] bossArray;
    public GameObject bossRoomBarrier;
    public Portal portal;
    private BossUI bossUI;

    [Header("Data")]
    public RoomData data;

    [Header("Sound")]
    public AudioClip bossBgmClip;

    [HideInInspector]
    public MonsterObjectPool monsterObjectPool;

    private void Awake()
    {
        camCollider2D = GetComponent<PolygonCollider2D>();
        monsterObjectPool = GetComponent<MonsterObjectPool>();
        IsBossAlive = true;
    }

    private void Start()
    {
        bossUI = UIManager.Instance.bossUI;

        cinemachineConfiner2D = GameManager.Instance.virtualCamera.GetComponent<CinemachineConfiner2D>();

        if (isBossRoom)
        {
            DisableBarrier();
        }

        // 플레이어가 죽은경우 배리어 비활성화
        GameManager.Instance.Player.healthSystem.OnDeath += DisableBarrier;
        
    }

    

    private void SwitchRoom()
    {
        GameManager.Instance.roomManager.currentRoomIdx = roomIdx;

        // 인접 방을 제외하고 비활성화

        for (int i = 0;  i < GameManager.Instance.roomManager.rooms.Length; i++)
        {
            // 현재방이면
            if (GameManager.Instance.roomManager.currentRoomIdx == i)
            {
                // 현재 룸을 넣어줌
                GameManager.Instance.roomManager.currentRoom = GameManager.Instance.roomManager.rooms[GameManager.Instance.roomManager.currentRoomIdx];

                Room currentRoom = GameManager.Instance.roomManager.currentRoom;

                // 보스룸인지 확인
                if (isBossRoom)
                {

                    // 보스가 살아있는 경우
                    if(IsBossAlive)
                    {
                        SoundManager.Instance.PlayBGM(bossBgmClip);

                        // bossUI.gameObject.SetActive(true);

                        bossUI.ShowBossUI();

                        // 보스방 배리어 타일 활성화
                        bossRoomBarrier.SetActive(true);

                    }
                    else
                    {
                        // 보스 지워줌 
                        foreach (Transform child in transform)
                        {
                            if (child.gameObject.layer == LayerMask.NameToLayer("Boss"))
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }
                    
                }
                
                continue;
            }
            
            GameManager.Instance.roomManager.rooms[i].gameObject.SetActive(false);
            GameManager.Instance.roomManager.rooms[i].isPlayerInRoom = false;
        }

        // ������ ���� ��� ����
        for (int i = 0; i < movableRoom.Length; i++)
        {
            movableRoom[i].gameObject.SetActive(true);
        }

        // ī�޶� ����
        cinemachineConfiner2D.m_BoundingShape2D = camCollider2D;

        isPlayerInRoom = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (!isPlayerInRoom && collision.tag == "Player")
       {
           SwitchRoom();
       }
    }

    void DisableBarrier()
    {
        if(bossRoomBarrier != null)
        bossRoomBarrier.SetActive(false);
    }
}
