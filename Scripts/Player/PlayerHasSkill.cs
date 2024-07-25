using System.Collections.Generic;
using UnityEngine;

public class PlayerHasSkill : MonoBehaviour
{
    public List<PlayerSkillSO> playerHasSkills = new List<PlayerSkillSO>();

    public void PlayerGetSkill(PlayerSkillSO playerSkillSO)
    {
        playerHasSkills.Add(playerSkillSO);
    }
}