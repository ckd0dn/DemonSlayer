using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillShopUI : UIBase
{
    Player player;
    SoulUI soulUI;

    public List<PlayerSkillSO> playerNoHasSkill = new List<PlayerSkillSO>();
    public List<GameObject> shopSkillImgs = new List<GameObject>();

    public GameObject shopSkillImg;
    public Image buyPanel;
    public GameObject buyBtn;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI lessCostTxt;

    private Renderer Renderer;

    private void Start()
    {
        soulUI = UIManager.Instance.soulUI;
        player = GameManager.Instance.Player; 
        UpdateShopSkill();
    }

    private void Update()
    {
        CanBuySkillColor();
    }

    public void UpdateShopSkill()
    {
        foreach(PlayerSkillSO skill in playerNoHasSkill)
        {   
            foreach(GameObject shop in shopSkillImgs)
            {
                if (shop.GetComponent<HasSkillInfo>().playerSkillSO == skill) return;
            }
            GameObject shopSkill = Instantiate(shopSkillImg, buyPanel.transform.position, Quaternion.identity, buyPanel.transform);
            shopSkill.GetComponent<Image>().sprite = skill.SkillIcon;
            shopSkill.GetComponent<HasSkillInfo>().playerSkillSO = skill;
            shopSkill.GetComponentInChildren<TextMeshProUGUI>().text = skill.SoulCost.ToString();
            shopSkill.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickSkill(shopSkill.GetComponent<HasSkillInfo>().playerSkillSO, shopSkill));
            shopSkillImgs.Add(shopSkill);
        }
    }

    public void OnClickSkill(PlayerSkillSO playerSkillSO, GameObject go)
    {
        int i = shopSkillImgs.IndexOf(go);
        PlayerPrefs.HasKey("CurrentIcon");
        PlayerPrefs.SetInt("CurrentIcon", i);
        skillDescription.text = $"{playerSkillSO.SkillInfo}";
        buyBtn.SetActive(true);
        buyBtn.GetComponent<HasSkillInfo>().playerSkillSO = playerSkillSO;
        lessCostTxt.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void OnClickBuyBtn()
    {
        PlayerSkillSO currentSkill = buyBtn.GetComponent<HasSkillInfo>().playerSkillSO;
        if(player.soulCount >= currentSkill.SoulCost)
        {
            player.soulCount -= currentSkill.SoulCost;
            playerNoHasSkill.Remove(currentSkill);
            shopSkillImgs[PlayerPrefs.GetInt("CurrentIcon")].SetActive(false);
            player.PlayerHasSkill.playerHasSkills.Add(currentSkill);          
            UpdateShopSkill();
            soulUI.UpdateSoulUI();
        }
        else
        {
            lessCostTxt.GetComponent<TextMeshProUGUI>().enabled = true;
            lessCostTxt.text = "보유 소울이 부족합니다";
        }
    }

    public void CanBuySkillColor()
    {
        foreach(GameObject skill in shopSkillImgs)
        {           
            if(skill.GetComponent<HasSkillInfo>().playerSkillSO.SoulCost <= player.soulCount)
            {
                skill.GetComponent<Image>().color = Color.white;
            }
            else
            {
                skill.GetComponent<Image>().color = Color.gray;
            }
        }
    }
        
}
