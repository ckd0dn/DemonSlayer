using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBtn : MonoBehaviour
{
    public TextMeshProUGUI saveLocationText; 
    public TextMeshProUGUI playTimeText; 
    public TextMeshProUGUI newGameText; 
    public TextMeshProUGUI soulCountText;
    public GameObject soulUI;

    private Datas[] datas;

    private void Start()
    {
        datas = DataManager.Instance.LoadAllData();
    }

    public void LoadAndStart(int idx)
    {
        // 불러올 데이터 파일 선택
        DataManager.Instance.SelectSaveData((ESaveFile)idx);

        if (datas[idx].isPlayIntro)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // 인트로 화면이동
            SceneManager.LoadScene("IntroScene");
        }     
    }

}
