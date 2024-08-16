using TMPro;
using UnityEngine;

public class SoulUI : UIBase
{
    public TextMeshProUGUI soultext;
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        UpdateSoulUI();
    }

    public void UpdateSoulUI()
    {
        soultext.text = player.soulCount.ToString();
    }
}
