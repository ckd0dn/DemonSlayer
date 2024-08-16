using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T[] mOrigin;

    protected List<T>[] mPool;

    private void Start()
    {
        mPool = new List<T>[mOrigin.Length];
        for (int i = 0; i < mOrigin.Length; ++i)
        {
            mPool[i] = new List<T>();
        }
    }

    public T GetFromPool(int id = 0)
    {
        for(int i = 0; i < mPool[id].Count; ++i)
        {
            if (!mPool[id][i].gameObject.activeInHierarchy)
            {
                mPool[id][i].gameObject.SetActive(true);
                return mPool[id][i];
            }
        }
        return MakeNewInstance(id);
    }

    protected virtual T MakeNewInstance(int id)
    {
        T newObj = Instantiate(mOrigin[id]);
        mPool[id].Add(newObj);
        return newObj;
    }
}
