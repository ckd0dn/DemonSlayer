using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    public Camera mapCamera;
    public MapData[] maps;
    private Room roomData;
    private Room currentRoom;
    private GameObject mapInfo;
    private MapController mapController;

    private WaitForSeconds mapUpdateTime = new WaitForSeconds(0.2f);

    private void Awake()
    {
        currentRoom = GameManager.Instance.roomManager.currentRoom;
        mapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
        mapInfo = transform.parent.gameObject;
        mapController = mapInfo.GetComponentInChildren<MapController>();
    }
    private void Start()
    {
        Init();
        StartCoroutine(UpdateMap());
    }

    private void Init()
    {
        int childCount = transform.childCount;
        maps = new MapData[transform.childCount];

        int i = 0;
        foreach (Transform child in this.transform)
        {            
            maps[i] = new MapData(child.GetComponent<SpriteRenderer>(), i);
            maps[i].SetAlpha(GameManager.Instance.roomManager.rooms[i].data.isPlayerVisited ? 0.5f : 0f);
            maps[i].isPlayerVisited = GameManager.Instance.roomManager.rooms[i].data.isPlayerVisited;
            i++;
        }
    }

    IEnumerator UpdateMap()
    {
        while(true)
        {
            yield return mapUpdateTime;

            if(!UIManager.Instance.onUI)
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    if (GameManager.Instance.roomManager.rooms[i].isPlayerInRoom)
                    {
                        maps[i].SetAlpha(1f);
                        maps[i].isPlayerVisited = true;
                        mapCamera.transform.position = maps[i].roomSprite.transform.position + new Vector3(0, 0, -10f);

                        GameManager.Instance.roomManager.rooms[i].data.isPlayerVisited = true;
                    }
                    else
                    {
                        if (maps[i].isPlayerVisited)
                        {
                            maps[i].SetAlpha(0.5f);
                        }
                    }
                }
            }
        }
    }


}
