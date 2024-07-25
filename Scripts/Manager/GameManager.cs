using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public RoomManager roomManager = new RoomManager();
    
    public TimeManager timeManager = new TimeManager();
    
    private Player _player;
    public Player Player { get; private set; }

    public GameObject virtualCamera;
    public Camera mapCamera;

    public ObjectPool pool;

    public AudioClip bgmClip;

    protected override void Awake()
    {
        base.Awake();
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        virtualCamera = GameObject.FindWithTag("VirtualCamera");
        mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
        pool = GetComponent<ObjectPool>();   
    }

    private void Start()
    {
        timeManager.ResetTime();
        roomManager.Init();        
        DataManager.Instance.LoadData();
        SoundManager.Instance.PlayBGM(bgmClip);
    }

    private void Update()
    {
        timeManager.CheckTime();
    }


}
