using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    
    // 처음 생성된 UI들을 담아놓을 딕셔너리.
    public Dictionary<string, UIBase> UIList = new Dictionary<string, UIBase>();
    public Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();

    public T LoadUI<T>() where T : UIBase
    {
        if(UIList.ContainsKey(typeof(T).Name))
        {
            return UIList[typeof(T).Name] as T;
        }

        var ui = Resources.Load<UIBase>($"UI/{typeof(T).Name}") as T;
        UIList.Add(ui.name, ui);
        return ui;
    }

    public GameObject LoadGameObject(string name) 
    {
        if (objectList.ContainsKey(name))
        {
            return objectList[name];
        }

        GameObject go = Resources.Load<GameObject>($"GameObject/{name}");
        objectList.Add(name, go);
        return go;
    }



}
