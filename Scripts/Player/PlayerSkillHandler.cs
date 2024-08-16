using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public PlayerSkillSO HolySlash;
    public PlayerSkillSO LightCut;
    public PlayerSkillSO HolyHeal;
    public PlayerSkillSO SaintHeal;
    public PlayerSkillSO SwordBuff;
    public PlayerSkillSO ShieldBuff;

    public void EquipSkillAdd(int index, PlayerSkillSO skillSO)
    {
        GameManager.Instance.Player.playerEquipSkill[index] = skillSO;
    }

    public void EquipSkillRemove(int index)
    {
        GameManager.Instance.Player.playerEquipSkill[index] = null;
    }

    public void HasSkillAdd(PlayerSkillSO skillSO)
    {
        GameManager.Instance.Player.PlayerHasSkill.playerHasSkills.Add(skillSO);
    }

    public void HasSkillRemove(PlayerSkillSO skillSO)
    {
        GameManager.Instance.Player.PlayerHasSkill.playerHasSkills.Remove(skillSO);
    }
}
