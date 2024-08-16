using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ObjectPool : MonoBehaviour
{
    // 오브젝트 풀 데이터를 정의할 데이터 모음 정의
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    //투사체 오브젝트풀
    public List<Pool> projectilePools;
    public Dictionary<GameObject, Queue<GameObject>> projectilePoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, objectPool);
        }

        //투사체 오브젝트풀
        projectilePoolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        foreach (var pool in projectilePools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            projectilePoolDictionary.Add(pool.prefab, objectPool);
        }

    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject projectileSpawnFromPool(GameObject projectile)
    {
        if (!projectilePoolDictionary.ContainsKey(projectile))
            return null;

        GameObject obj = projectilePoolDictionary[projectile].Dequeue();
        projectilePoolDictionary[projectile].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }
}