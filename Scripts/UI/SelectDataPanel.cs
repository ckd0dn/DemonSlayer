using UnityEngine;

public class SelectDataPanel : MonoBehaviour
{
    public LoadBtn[] loadBtns;

    TimeManager timeManager = new TimeManager();

    private void Start()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        Datas[] datas = DataManager.Instance.LoadAllData();

        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].playTime > 0)
            {
                // 저장 위치
                loadBtns[i].saveLocationText.text = datas[i].roomManagerData.lastCheckPointName;

                // 플레이타임도 추가
                string playTimeText = "플레이 시간 : " + timeManager.GetFormattedPlayTime(datas[i].playTime);

                loadBtns[i].playTimeText.text = playTimeText;
                // 소울
                string soulCountText = datas[i].playerData.soulCount.ToString();
                loadBtns[i].soulCountText.text = soulCountText;
                loadBtns[i].soulUI.SetActive(true);

            }
            else
            {
                loadBtns[i].newGameText.gameObject.SetActive(true);
                loadBtns[i].saveLocationText.text = "";
                loadBtns[i].playTimeText.text = "";
                loadBtns[i].soulCountText.text = "";
                loadBtns[i].soulUI.SetActive(false);
            }
        }
    }
}
