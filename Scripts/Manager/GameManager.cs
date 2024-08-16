using Cinemachine;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public RoomManager roomManager = new RoomManager();
    public TimeManager timeManager = new TimeManager();
    public ItemManager itemManager = new ItemManager();

    public CheckpointManager checkpointManager;
    private Player _player;
    public Player Player { get; private set; }

    public GameObject virtualCamera;
    public Camera mapCamera;

    public ObjectPool pool;

    public DamageTextPool damageTextPool;

    public AudioClip bgmClip;

    public CameraUtil cameraUtil;
    public bool MonsterMove;

    //
    public CheckPointUI TutorialCheckPoint;
    protected override void Awake()
    {
        base.Awake();
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        virtualCamera = GameObject.FindWithTag("VirtualCamera");
        mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
        pool = GetComponent<ObjectPool>();
        damageTextPool = GameObject.FindWithTag("DamageTextPool").GetComponent<DamageTextPool>();
        cameraUtil = FindAnyObjectByType<CameraUtil>();
        MonsterMove = true;
    }

    private void Start()
    {
        timeManager.ResetTime();
        roomManager.Init();
        checkpointManager = CheckpointManager.Instance;
        checkpointManager.Init();
        itemManager.Init();
        SoundManager.Instance.PlayBGM(bgmClip);

        DataManager.Instance.LoadData();
    }

    private void Update()
    {
        timeManager.CheckTime();
    }


}
