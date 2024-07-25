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

    public void EquipSkillChange(int index, PlayerSkillSO skillSO)
    {
        GameManager.Instance.Player.playerEquipSkill[index] = skillSO;
    }

    public void HasSkillChange(PlayerSkillSO skillSO)
    {
        GameManager.Instance.Player.PlayerHasSkill.playerHasSkills.Add(skillSO);
    }
}
