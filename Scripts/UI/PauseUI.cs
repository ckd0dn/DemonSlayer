using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    Player player;
    UIManager uiManager;

    public Button ContinueBtn;
    public FadeEffect fade;
    public GameObject pauseui;
    public GameObject ErrorMessage;
    public GameObject startScenebtn;
    public GameObject exitbtn;
    public GameObject Settingbtn;

    private void OnEnable()
    {
        ContinueBtn.Select();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        uiManager = UIManager.Instance;
        Settingbtn = SoundManager.Instance.SoundSetting;
        ObjectMessageFalse();
    }

    public void ObjectMessageFalse()
    {
        startScenebtn.SetActive(false);
        ErrorMessage.SetActive(false);
        exitbtn.SetActive(false);
        pauseui.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
    public void ReturnCheckPoint()
    {
        if (NoSavecheckPointPosition())
        {
            StartCoroutine(NoSavePositionMessage());
        }
        else
        {
            StartCoroutine(ReturnCheckPointCoroutine());
        }
    }

    private IEnumerator ReturnCheckPointCoroutine()
    {
        pauseui.SetActive(false);
        yield return StartCoroutine(fade.FadeOut());
        yield return new WaitForSecondsRealtime(1f);
        player.Respawn();
        yield return StartCoroutine(fade.FadeIn());
        Resume();
        pauseui.SetActive(true);
        uiManager.onUI = false;
    }

    public bool NoSavecheckPointPosition()
    {
        if (GameManager.Instance.roomManager.checkPointPosition == Vector3.zero)
        {
            return true; 
        }
        return false; 
    }

    private IEnumerator NoSavePositionMessage()
    {
        ErrorMessage.SetActive(true);
        pauseui.SetActive(false);
        yield return new WaitForSecondsRealtime(1f); //time.taiesclae 이 0이면 안돌아감 waitforseonds안먹음
        ErrorMessage.SetActive(false);
        pauseui.SetActive(true);
        yield return null;
    }
    public void OpenSoundSettingBtn()
    {
        Settingbtn.SetActive(true);
    }

    public void OpenStartSceneBtn()
    {
        startScenebtn.SetActive(true);
        pauseui.SetActive(false);
    }
    public void CloseStartSceneBtn()
    {
        startScenebtn.SetActive(false);
        pauseui.SetActive(true);
    }

    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");

        UIManager.Instance.DestroyManager();
        ResourceManager.Instance.DestroyManager();
        DataManager.Instance.DestroyManager();
        GameManager.Instance.DestroyManager();
        SoundManager.Instance.DestroyManager();

        Time.timeScale = 1f;
    }
    public void ExitBtn()
    {
        exitbtn.SetActive(true);
        pauseui.SetActive(false);
    }

    public void CloseExitBtn()
    {
        exitbtn.SetActive(false);
        pauseui.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
