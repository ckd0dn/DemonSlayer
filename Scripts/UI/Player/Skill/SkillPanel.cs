using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    SkillUI skillUI;

    public HasSkillInfo HasSkillInfo;
    public SkillEquipMenu skillEquip;
    public List<PlayerSkillSO> playerHasSkillInEquipMenu = new List<PlayerSkillSO>();
    public List<GameObject> HasSkillGameObjects = new List<GameObject>(); 

    public List<PlayerSkillSO> equipSkills;
    public List<PlayerSkillSO> HasSkills;
    public List<Image> images;

    public GameObject hasSkillPanel;
    public GameObject defaultImgPrefab;

    private Color TransparencyColor = new Color(1, 1, 1, 0);
    private Color colored = new Color(1, 1, 1, 1);

    private void OnEnable()
    {
        skillUI = UIManager.Instance.skillUI;
        UpdateSkillEquip();
        UpdatePlayerHasSkill();
    }

    private void Start()
    {
        skillEquip = GetComponent<SkillEquipMenu>();
        HasSkills = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
        equipSkills = GameManager.Instance.Player.playerEquipSkill;
        StartEquipSkillAlpha(0f);

        UpdateSkillEquip(); 
        UpdatePlayerHasSkill();
    }

    private void StartEquipSkillAlpha(float alpha)
    {
        for (int i = 0; i < images.Count; i++)
        {
            Color color = images[i].color;
            color.a = Mathf.Clamp01(alpha);
            images[i].color = color;
        }
    }

    public void UpdateSkillEquip()
    {        
        equipSkills = GameManager.Instance.Player.playerEquipSkill;
        for (int i = 0; i < equipSkills.Count; i++)
        {
            if (equipSkills[i] == null)
            {
                images[i].sprite = null;
                images[i].color = TransparencyColor;
            }
            else
            {
                images[i].sprite = equipSkills[i].SkillIcon;
                images[i].color = colored;
            }
        }
        skillUI?.OnSkillUpdate();
    }

    public void UpdatePlayerHasSkill()
    {
        HasSkills = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
        foreach (var skill in HasSkills)
        {
            // 스킬 버튼에 가지고 있는 스킬이 없다면
            if (!CheckHasSkillGameObjects(skill))
            {
                //버튼 만들기
                GameObject hasSkill = Instantiate(defaultImgPrefab, hasSkillPanel.transform.localPosition, Quaternion.identity, hasSkillPanel.transform);
                hasSkill.GetComponent<Image>().sprite = skill.SkillIcon;
                hasSkill.GetComponent<HasSkillInfo>().playerSkillSO = skill;
                hasSkill.GetComponent<Button>().onClick.AddListener(() => skillEquip.OnSkillMoveEquip(hasSkill.GetComponent<HasSkillInfo>().playerSkillSO));
                HasSkillGameObjects.Add(hasSkill);
            }
           
        };
    }

    private bool CheckHasSkillGameObjects(PlayerSkillSO skill)
    {
        bool isHasSkillGobj = false;

        foreach (var skillOb in HasSkillGameObjects)
        {
            if(skill == skillOb.GetComponent<HasSkillInfo>().playerSkillSO)
            {
                isHasSkillGobj = true;
                return isHasSkillGobj;
            }
        }

        return isHasSkillGobj;
    }
}




//int index = 0;
//GameObject hasSkill = Instantiate(defaultImgPrefab, hasSkillPanel.transform.localPosition, Quaternion.identity, hasSkillPanel.transform);
//hasSkill.GetComponent<Image>().sprite = skill.SkillIcon;
//hasSkill.GetComponent<HasSkillInfo>().playerSkillSO = skill;
////hasSkill.GetComponent<Button>().onClick.AddListener(() => skillEquip.OnSkillMoveEquip(index++));