using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public RelicsItem[] items;


    public void Init()
    {
        GameObject[] fieldItmes = GameObject.FindGameObjectsWithTag("Relics");

        System.Array.Sort(fieldItmes, (x, y) => x.name.CompareTo(y.name));

        items = new RelicsItem[fieldItmes.Length];

        for (int i = 0; i < fieldItmes.Length; i++)
        {
            items[i] = fieldItmes[i].GetComponent<RelicsItem>();
        }

    }


    public void DestroyItem()
    {
        foreach (RelicsItem item in items)
        {
            if (item.isGet) GameObject.Destroy(item.gameObject);
        }
    }

}
