using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClearBtn : MonoBehaviour
{
    public SelectDataPanel selectDataPanel;

    public void ClearData(int idx)
    {
        DataManager.Instance.SelectClearData((ESaveFile)idx);
        // 데이터 선택 파넬 UI 업데이트
        selectDataPanel.UpdateData();
    }
}
