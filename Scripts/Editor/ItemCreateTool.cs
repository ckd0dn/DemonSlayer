using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public enum CreateItemType
{
    Equip,
    Passive
}

public enum CreatePlayerActive
{
    None,
    DoubleJump,
    Dash
}
public class ItemCreateTool : EditorWindow
{
    public Sprite itemImage;
    public string itemName;        
    public string description; 
    public int requireCurrency;
    private GameObject ItemPrefab;
    public CreateItemType createitemType;
    public CreatePlayerActive createplayerActive;

    private int hp;
    private float speed;
    private float damage;

    [MenuItem("MyTool/Item Creator")]
    public static void ShowWindow()
    {
        GetWindow<ItemCreateTool>("Item Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("아이템 생성 툴 ", EditorStyles.boldLabel);

        // StatsSO 필드들
        hp = EditorGUILayout.IntField("HP", hp);
        speed = EditorGUILayout.FloatField("Speed", speed);
        damage = EditorGUILayout.FloatField("damage", damage);

        // ItemSO 필드들
        itemImage = (Sprite)EditorGUILayout.ObjectField("itemImage", itemImage, typeof(Sprite), false);
        itemName = EditorGUILayout.TextField("Item Name", itemName);
        description = EditorGUILayout.TextField("description", description);
        ItemPrefab = (GameObject)EditorGUILayout.ObjectField("Item Prefab", ItemPrefab, typeof(GameObject), false);
        createitemType = (CreateItemType)EditorGUILayout.EnumPopup("item Type", createitemType);
        createplayerActive = (CreatePlayerActive)EditorGUILayout.EnumPopup("Player Active", createplayerActive);

        // Create 버튼
        if (GUILayout.Button("Create Item Stats"))
        {
            CreateItemStats();
        }
    }

    private void CreateItemStats()
    {
        // 새로운 StatsSO 객체 생성
        StatsSO newStats = ScriptableObject.CreateInstance<StatsSO>();
        newStats.hp = hp;
        newStats.speed = speed;
        newStats.damage = damage;

        // 새로운 ItemStatsSO 객체 생성
        ItemSO newItemStat = ScriptableObject.CreateInstance<ItemSO>();
        newItemStat.itemImage = itemImage;
        newItemStat.itemName = itemName;
        newItemStat.description = description;
        newItemStat.ItemPrefab = ItemPrefab;
        newItemStat.itemType = (ItemType)createitemType;
        newItemStat.playerActive = (PlayerActive)createplayerActive;


        // StatsSO 필드 연결
        newItemStat.hp = hp;
        newItemStat.speed = speed;
        newItemStat.damage = damage;

        // ItemStatsSO 객체를 프로젝트에 저장
        string monsterStatsPath = "Assets/Data/Stats/Item";
        if (!AssetDatabase.IsValidFolder(monsterStatsPath))
        {
            AssetDatabase.CreateFolder("Assets/Data/Item", "Item");
        }

        string monsterStatsAssetPath = $"{monsterStatsPath}/{itemName}SO.asset";
        AssetDatabase.CreateAsset(newItemStat, monsterStatsAssetPath);

        // 아이템 프리팹 생성
        CreateItemPrefab(newItemStat);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 생성 후 필드 초기화
        itemImage = null;
        itemName = "";
        description = "";
        ItemPrefab = null;
        hp = 0;
        speed = 0;
        damage = 0;

        EditorUtility.DisplayDialog("아이템 생성", "아이템이 생성되었습니다.", "OK");
    }

    private void CreateItemPrefab(ItemSO stats)
    {
        if (stats.ItemPrefab == null)
        {
            EditorUtility.DisplayDialog("에러", "프리팹을 넣어주세요.", "OK");
            return;
        }

        // 새로운 GameObject 생성 및 설정 적용
        GameObject newItem = Instantiate(stats.ItemPrefab);
        newItem.name = stats.itemName;

        // 필요한 컴포넌트를 추가하고 설정을 적용
        RelicsItem ItemComponent = newItem.GetComponent<RelicsItem>();
        if (ItemComponent == null)
        {
            ItemComponent = newItem.AddComponent<RelicsItem>();
        }
        ItemComponent.itemSO = stats; // 스탯을 설정


        // 생성된 GameObject를 프리팹으로 저장
        string path = "Assets/Prefabs/Item";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Item", "Item");
        }

        string prefabPath = $"{path}/{stats.itemName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(newItem, prefabPath);
        DestroyImmediate(newItem);
    }
}
