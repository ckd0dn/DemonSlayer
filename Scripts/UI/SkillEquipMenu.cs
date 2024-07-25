using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillEquipMenu : UIBase
{
    public TextMeshProUGUI skillEquipTxt;
    public TextMeshProUGUI skillDescription;

    SkillPanel skillPanel;

    GameObject dragImage;

    private bool isEmptySlotBtn = false;
    private int i = 0;

    List<PlayerSkillSO> playerEquipSkills;
    List<PlayerSkillSO> playerHasSkills;
    

    private void Start()
    {
        skillPanel = GetComponent<SkillPanel>();
        playerEquipSkills = GameManager.Instance.Player.playerEquipSkill;
        playerHasSkills = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
    }

    public void OnEquipSkill(int index)
    {
        i = index;
        isEmptySlotBtn = true;
        skillEquipTxt.text = " 이 칸에 장착하실 스킬을 선택해 주세요 ";
    }

    public void OnSkillMoveEquip(PlayerSkillSO playerSkillSO)
    {
        if(isEmptySlotBtn)
        {           
            GameManager.Instance.Player.PlayerSkillHandler.EquipSkillChange(i, playerSkillSO);
            skillPanel.UpdateSkillEquip();
            isEmptySlotBtn = false;
            skillDescription.text = playerSkillSO.SkillInfo;
        }
        else
        {
            skillDescription.text = playerSkillSO.SkillInfo;
        }
    }

}
