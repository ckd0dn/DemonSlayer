using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MonsterCreateTool : EditorWindow
{
    private string monsterName;
    public float[] attackDetectRange; //공격 감지범위
    public float[] attackDistance; //공격 실행
    private float detectRange;
    private MonsterClassType monsterClassType;
    private GameObject monsterPrefab;

    private int hp;
    private float speed;
    private float damage;

    [MenuItem("MyTool/Monster Stats Creator")]
    public static void ShowWindow()
    {
        GetWindow<MonsterCreateTool>("Monster Stats Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("몬스터 생성 툴", EditorStyles.boldLabel);

        // StatsSO 필드들
        hp = EditorGUILayout.IntField("HP", hp);
        speed = EditorGUILayout.FloatField("Speed", speed);
        damage = EditorGUILayout.FloatField("damage", damage);

        // MonsterStatsSO 필드들
        monsterName = EditorGUILayout.TextField("Monster Name", monsterName);

        for (int i = 0; i < attackDetectRange.Length; i++)
        {
            attackDetectRange[i] = EditorGUILayout.FloatField($"Attack Range {i + 1}", attackDetectRange[i]);
        }

        detectRange = EditorGUILayout.FloatField("detectRange", detectRange);
        monsterClassType = (MonsterClassType)EditorGUILayout.EnumPopup("Monster Type", monsterClassType);
        monsterPrefab = (GameObject)EditorGUILayout.ObjectField("Monster Prefab", monsterPrefab, typeof(GameObject), false);

        // Create 버튼
        if (GUILayout.Button("Create Monster Stats"))
        {
            CreateMonsterStats();
        }
    }

    private void CreateMonsterStats()
    {
        // 새로운 StatsSO 객체 생성
        StatsSO newStats = ScriptableObject.CreateInstance<StatsSO>();
        newStats.hp = hp;
        newStats.speed = speed;
        newStats.damage = damage;

        // 새로운 MonsterStatsSO 객체 생성
        MonsterStatsSO newMonsterStats = ScriptableObject.CreateInstance<MonsterStatsSO>();
        newMonsterStats.monsterName = monsterName;
        newMonsterStats.attackDetectRange = attackDetectRange;
        newMonsterStats.detectRange = detectRange;
        newMonsterStats.monsterClassType = monsterClassType;
        newMonsterStats.monsterPrefab = monsterPrefab;

        // StatsSO 필드 연결
        newMonsterStats.hp = hp;
        newMonsterStats.speed = speed;
        newMonsterStats.damage = damage;

        // MonsterStatsSO 객체를 프로젝트에 저장
        string monsterStatsPath = "Assets/Data/Stats/Monster";
        if (!AssetDatabase.IsValidFolder(monsterStatsPath))
        {
            AssetDatabase.CreateFolder("Assets/Data/Stats", "Monster");
        }

        string monsterStatsAssetPath = $"{monsterStatsPath}/{monsterName}SO.asset";
        AssetDatabase.CreateAsset(newMonsterStats, monsterStatsAssetPath);

        // 몬스터 프리팹 생성
        CreateMonsterPrefab(newMonsterStats);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 생성 후 필드 초기화
        monsterName = "";
        attackDetectRange = new float[3];
        detectRange = 0;
        monsterClassType = MonsterClassType.Basic;
        monsterPrefab = null;
        hp = 0;
        speed = 0;
        damage = 0;

        EditorUtility.DisplayDialog("몬스터 생성", "몬스터 종류 스크립트랑 애니메이션을 설정해야합니다.", "OK");
    }

    private void CreateMonsterPrefab(MonsterStatsSO stats)
    {
        if (stats.monsterPrefab == null)
        {
            EditorUtility.DisplayDialog("에러", "프리팹을 넣어주세요.", "OK");
            return;
        }

        // 새로운 GameObject 생성 및 설정 적용
        GameObject newMonster = Instantiate(stats.monsterPrefab);
        newMonster.name = stats.monsterName;

        // 필요한 컴포넌트를 추가하고 설정을 적용
        Monster monsterComponent = newMonster.GetComponent<Monster>();
        StatHandler monsterStatHandler = newMonster.GetComponent<StatHandler>();
        if (monsterComponent == null)
        {
            monsterComponent = newMonster.AddComponent<Monster>();
        }
        monsterComponent.stats = stats; // 스탯을 설정
        monsterStatHandler.baseStat.statsSO = stats;


        // 생성된 GameObject를 프리팹으로 저장
        string path = "Assets/Prefabs/Monster";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Monster");
        }

        string prefabPath = $"{path}/{stats.monsterName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(newMonster, prefabPath);
        DestroyImmediate(newMonster);
    }
}
