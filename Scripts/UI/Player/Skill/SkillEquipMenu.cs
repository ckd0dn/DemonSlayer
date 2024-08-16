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
    public GameObject UnEquipBtn;

    SkillPanel skillPanel;
    //ShaderOutlineToggle shaderOutlineToggle;

    GameObject dragImage;

    private bool isEmptySlotBtn = false;
    private int i = 0;

    List<PlayerSkillSO> playerEquipSkills;
    List<PlayerSkillSO> playerHasSkills;
    

    private void Start()
    {
        //shaderOutlineToggle = GetComponent<ShaderOutlineToggle>();
        skillPanel = GetComponent<SkillPanel>();
        playerEquipSkills = GameManager.Instance.Player.playerEquipSkill;
        playerHasSkills = GameManager.Instance.Player.PlayerHasSkill.playerHasSkills;
    }

    public void OnEquipSkill(int index)
    {
        if (playerEquipSkills[index] == null)
        {
            i = index;
            isEmptySlotBtn = true;
            skillEquipTxt.text = " 이 칸에 장착하실 스킬을 선택해 주세요 ";
        }
        else
        {
            i = index;
            UnEquipBtn.SetActive(true);
            skillDescription.text = playerEquipSkills[index].SkillInfo;
        }
    }

    public void UnEquipBtnOnClick()
    {
        //GameManager.Instance.Player.PlayerSkillHandler.HasSkillAdd(playerEquipSkills[i]);
        GameManager.Instance.Player.PlayerSkillHandler.EquipSkillRemove(i);
        skillPanel.UpdateSkillEquip();
        //skillPanel.UpdatePlayerHasSkill();
        UnEquipBtn.SetActive(false);
    }

    public void OnSkillMoveEquip(PlayerSkillSO playerSkillSO)
    {
        if(isEmptySlotBtn)
        {           
            GameManager.Instance.Player.PlayerSkillHandler.EquipSkillAdd(i, playerSkillSO);
            //GameManager.Instance.Player.PlayerSkillHandler.HasSkillRemove(playerSkillSO);
            skillPanel.UpdateSkillEquip();
            //skillPanel.UpdatePlayerHasSkill();
            isEmptySlotBtn = false;
            skillDescription.text = playerSkillSO.SkillInfo;
            skillEquipTxt.text = "";
        }
        else
        {
            skillDescription.text = playerSkillSO.SkillInfo;
        }
    }

}
