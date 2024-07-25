using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public HasSkillInfo HasSkillInfo;
    public SkillEquipMenu skillEquip;
    public List<PlayerSkillSO> playerHasSkillInEquipMenu = new List<PlayerSkillSO>();

    public List<PlayerSkillSO> equipSkills;
    public List<PlayerSkillSO> HasSkills;
    public List<Image> images;

    public GameObject hasSkillPanel;
    public Button defaultImgPrefab;


    private void Start()
    {
        skillEquip = GetComponent<SkillEquipMenu>();
        HasSkills = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
        equipSkills = GameManager.Instance.Player.playerEquipSkill;
        UpdateSkillEquip();
        UpdatePlayerHasSkill();
    }

    public void UpdateSkillEquip()
    {
        for (int i = 0; i < equipSkills.Count; i++)
        {
            images[i].sprite = equipSkills[i].SkillIcon;
        }
    }

    public void UpdatePlayerHasSkill()
    {
        foreach (var skill in HasSkills)
        {
            Button hasSkill = Instantiate(defaultImgPrefab, hasSkillPanel.transform.localPosition, Quaternion.identity, hasSkillPanel.transform);
            hasSkill.GetComponent<Image>().sprite = skill.SkillIcon;
            hasSkill.GetComponent<HasSkillInfo>().playerSkillSO = skill;
            hasSkill.GetComponent<Button>().onClick.AddListener(() => skillEquip.OnSkillMoveEquip(hasSkill.GetComponent<HasSkillInfo>().playerSkillSO));
        };
    }
}


//int index = 0;
//GameObject hasSkill = Instantiate(defaultImgPrefab, hasSkillPanel.transform.localPosition, Quaternion.identity, hasSkillPanel.transform);
//hasSkill.GetComponent<Image>().sprite = skill.SkillIcon;
//hasSkill.GetComponent<HasSkillInfo>().playerSkillSO = skill;
////hasSkill.GetComponent<Button>().onClick.AddListener(() => skillEquip.OnSkillMoveEquip(index++));