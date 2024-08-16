using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUI : UIBase
{
    private void OnEnable()
    {
        UIManager.Instance.onUIEvent += SetActiveMiniMap;
    }

    private void SetActiveMiniMap(bool onUI)
    {
        gameObject.SetActive(!onUI);
    }
}
