using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LodingText : MonoBehaviour
{
    public TextMeshProUGUI lodingText;
    private string[] tips;

    public float blinkSpeed = 1.0f;

    private WaitForSeconds blinkDurantion = new WaitForSeconds(0.1f);

    public void Start()
    {
        tips = new string[]
        {
            "TIP: 스킵은 'Speac bar'키로 \r\n진행 할 수 있습니다.",
            "TIP: 맵 곳곳에 숨겨져 있는 아이템이\r\n 존재 합니다.",
            "TIP: 패시브 아이템은 캐릭터에게 \r\n영구적으로 능력치를 줄 수 있습니다.",
            "TIP: 킹슬라임을 처치하면 높게 뛸 수 있는 \r\n아이템을 드랍한다고 합니다."
        };

        SetRandomTip();
    }

    private void Update()
    {
        Color color = lodingText.color;
        color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
        lodingText.color = color;
    }

    private void SetRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length);

        lodingText.text = tips[randomIndex];
    }
}
