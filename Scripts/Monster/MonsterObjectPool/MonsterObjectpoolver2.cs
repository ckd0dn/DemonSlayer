using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPoolver2 : MonoBehaviour
{
    public Transform[] spawnPoints; // 스폰 위치 배열
    public GameObject[] monsterPrefabs; // 몬스터 프리팹 배열

    public float spawnInterval = 3f; // 몬스터 스폰 간격
    public float initialSpawnDelay = 2f; // 초기 스폰 지연 시간

    private List<GameObject> monsterPool; // 몬스터 오브젝트 풀
    private List<int> availableSpawnPoints; // 스폰 가능한 위치 인덱스 리스트

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 초기화 및 코루틴 시작
        Initialize();
        Invoke("StartSpawning", initialSpawnDelay);
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 코루틴 중지
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // 모든 활성화된 몬스터 비활성화
        foreach (GameObject monster in monsterPool)
        {
            if (monster.activeSelf)
            {
                monster.SetActive(false);
            }
        }
    }

    private void Initialize()
    {
        // 몬스터 오브젝트 풀 초기화
        if (monsterPool == null)
        {
            monsterPool = new List<GameObject>();
        }
        else
        {
            monsterPool.Clear();
        }

        // 몬스터를 초기에 전부 스폰
        SpawnAllMonsters();

        // 스폰 가능한 위치 인덱스 리스트 초기화
        if (availableSpawnPoints == null)
        {
            availableSpawnPoints = new List<int>();
        }
        else
        {
            availableSpawnPoints.Clear();
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }
    }

    private void StartSpawning()
    {
        // 코루틴 시작
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnMonsters());
        }
    }

    private void SpawnAllMonsters()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // 몬스터를 오브젝트 풀에서 가져오거나 생성
            GameObject monster = GetOrCreateMonsterFromPool();

            // 몬스터를 스폰 위치에 스폰
            monster.transform.position = spawnPoint.position;
            monster.SetActive(true);
        }
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 스폰 가능한 위치가 있는지 확인
            if (availableSpawnPoints.Count == 0)
                continue;

            // 스폰 위치 인덱스를 랜덤하게 선택
            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
            int spawnPointIndex = availableSpawnPoints[spawnIndex];

            // 해당 스폰 위치에 몬스터가 이미 스폰되어 있는지 확인
            if (IsMonsterSpawnedAtPoint(spawnPointIndex))
                continue;

            // 몬스터를 오브젝트 풀에서 가져오거나 생성
            GameObject monster = GetOrCreateMonsterFromPool();

            // 몬스터를 선택된 스폰 위치에 스폰
            monster.transform.position = spawnPoints[spawnPointIndex].position;
            monster.SetActive(true);

            // 스폰된 위치 인덱스를 스폰 가능한 위치 인덱스 리스트에서 제거
            availableSpawnPoints.RemoveAt(spawnIndex);
        }
    }

    private bool IsMonsterSpawnedAtPoint(int spawnPointIndex)
    {
        bool hasMonster = false;

        // 해당 스폰 위치에 몬스터가 있는지 검사
        GameObject monster = monsterPool.Find(m => m != null && m.activeSelf && m.transform.position == spawnPoints[spawnPointIndex].position);
        if (monster == null)
        {
            hasMonster = true;
        }

        return hasMonster;
    }

    // 몬스터를 오브젝트 풀에서 가져오거나 생성
    private GameObject GetOrCreateMonsterFromPool()
    {
        GameObject monster = monsterPool.Find(m => !m.activeSelf);
        if (monster == null)
        {
            foreach (GameObject monsterPrefab in monsterPrefabs)
            {
                GameObject newMonster = Instantiate(monsterPrefab);
                newMonster.SetActive(false);
                monsterPool.Add(newMonster);
            }
            monster = monsterPool.Find(m => !m.activeSelf);
        }
        return monster;
    }
}
