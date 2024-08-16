using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : UIBase
{
    private Vector3 originBossHpBarLocalScale;
    [SerializeField] RectTransform bossHpBar;
    [SerializeField] RectTransform bossHpBarFiller;
    private Image bossHpBarFillerImage;
    [SerializeField] private float stretchHpSpeed;
    [SerializeField] private float appearNameSpeed;
    [SerializeField] private TextMeshProUGUI bossNameText;

    private float maxBossHp;
    private float bossHp;

    private void Awake()
    {
        bossHpBarFillerImage = bossHpBarFiller.GetComponent<Image>();
        originBossHpBarLocalScale = bossHpBar.localScale;
    }

    void Start()
    {
        gameObject.SetActive(false);

        // 플레이어가 죽을때 보스 UI 비활성화
        GameManager.Instance.Player.healthSystem.OnDeath += DisableBossUI;
    }

    //OnEnable 에서 보스데이터를 세팅해줄 수 있을것같다
    //private void OnEnable()
    //{
    //    SettingBossData();
    //}

    public void SettingBossData()
    {
        if (GameManager.Instance.roomManager.currentRoom == null) return;

        StatHandler bossStats = GameManager.Instance.roomManager.currentRoom.bossArray[0].GetComponent<StatHandler>();
        MonsterStatsSO bossStatsSO = (MonsterStatsSO)bossStats.CurrentStat.statsSO;

        bossNameText.text = bossStatsSO.monsterName;
        maxBossHp = bossStatsSO.hp;
        bossHp = bossStatsSO.hp;
        bossHpBarFillerImage.fillAmount = 1;
    }

    public void SettingBossData(StatHandler statHandler)
    {
        MonsterStatsSO bossStatsSO = (MonsterStatsSO)statHandler.CurrentStat.statsSO;

        bossNameText.text = bossStatsSO.monsterName;
        maxBossHp = bossStatsSO.hp;
        bossHp = bossStatsSO.hp;
        bossHpBarFillerImage.fillAmount = 1;
    }

    public void ShowBossUI()
    {
        SettingBossData(); 

        gameObject.SetActive (true);

        StartCoroutine(AppearBossHpBar());
        StartCoroutine(AppearBossText());
    }

    public void ShowBossUI(StatHandler statHandler)
    {
        SettingBossData(statHandler);

        gameObject.SetActive(true);

        StartCoroutine(AppearBossHpBar());
        StartCoroutine(AppearBossText());
    }

    public void HideBossUI()
    {
        StartCoroutine(DisAppearBossHpBar());  
    }

    public void DisableBossUI()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator AppearBossHpBar()
    {
        Vector3 newScale = new Vector3 (originBossHpBarLocalScale.x, originBossHpBarLocalScale.y, 1f);
        float duration = newScale.x;

        for (float timer = 0; timer < duration; timer += Time.deltaTime * stretchHpSpeed / 100f)
        {
            newScale.x = timer;
            bossHpBar.localScale = newScale;
            bossHpBarFiller.localScale = newScale;
            yield return null;
        }
    }

    private IEnumerator AppearBossText()
    {
        Color color = bossNameText.color;
        int alpha = 255;

        for (float timer = 0; timer < alpha; timer += Time.deltaTime * appearNameSpeed)
        {
            color.a = (timer / alpha);
            bossNameText.color = color;
            yield return null;
        }

        color.a = 1;
        bossNameText.color = color;
    }

    private IEnumerator DisAppearBossHpBar()
    {
        Vector3 newScale = new Vector3(bossHpBar.localScale.x, bossHpBar.localScale.y, 1f);
        float duration = bossHpBar.localScale.x;

        for (float timer = duration; timer > 0; timer -= Time.deltaTime * stretchHpSpeed / 100f)
        {
            newScale.x = timer;
            bossHpBar.localScale = newScale;
            bossHpBarFiller.localScale = newScale;
            yield return null;
        }

        newScale.x = 0;
        bossHpBar.localScale = newScale;
        bossHpBarFiller.localScale = newScale;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    public void UpdateBossHp(float hp)
    {
        bossHp = hp;
        bossHpBarFillerImage.fillAmount = (bossHp / maxBossHp);

        // hp가 0 일때 보스 UI 비활성화
        if(hp <= 0)
        {
            HideBossUI();

            // 보스방 배리어 타일 비활성화
            if(GameManager.Instance.roomManager.currentRoom.bossRoomBarrier != null)
            GameManager.Instance.roomManager.currentRoom.bossRoomBarrier.SetActive(false);
        }
    }


}
