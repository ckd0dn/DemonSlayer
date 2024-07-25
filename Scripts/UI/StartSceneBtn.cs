using UnityEngine;
using UnityEngine.UI;

public class StartSceneBtn : UIBase
{
    public GameObject SaveLoad;
    public GameObject soundSetting;

    private void Start()
    {
        soundSetting = SoundManager.Instance.SoundSetting;
    }
    public void OpenLoadBtn()
    {
        SaveLoad.SetActive(true);
    }

    public void CloseLoadBtn()
    {
        SaveLoad.SetActive(false);
    }

    public void OpenSettingBtn()
    {
        soundSetting.SetActive(true);
    }
    public void ClolseSettingBtn()
    {
        soundSetting.SetActive(false);
    }
}
