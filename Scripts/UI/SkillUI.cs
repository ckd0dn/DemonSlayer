using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : UIBase
{
    Player player;
    Sprite icon;
    Image image;
    SkillUI skillUI;
    public List<PlayerSkillSO> skillList;
    public List<Image> skills;
    public GameObject firstSlot;
    public GameObject secondSlot;

    private float skillSwapSpeed = 0.5f;
    private bool isSwap = false;

    private Vector3 firstSkillSlot;
    private Vector3 secondSkillSlot;

    private void Start()
    {
        skillUI = GetComponent<SkillUI>();
        player = GameManager.Instance.Player;
        image = GetComponent<Image>();
        icon = image.sprite;
        skillList = player.playerEquipSkill;
        firstSkillSlot = firstSlot.transform.position;
        secondSkillSlot = secondSlot.transform.position;
        OnSkillUpdate();
    }

    public void OnSkillUpdate()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            skills[i].GetComponent<Image>().sprite = skillList[i].SkillIcon;
        }

    } 

    public void SkillSlotMover()
    {
        if(!isSwap)
        {
            secondSlot.transform.DOMove(firstSkillSlot , skillSwapSpeed).SetEase(Ease.InOutQuad);
            firstSlot.transform.DOMove(secondSkillSlot , skillSwapSpeed).SetEase(Ease.InOutQuad);
            isSwap = true;
            secondSlot.transform.SetAsLastSibling();
        }
        else
        {
            secondSlot.transform.DOMove(secondSkillSlot, skillSwapSpeed).SetEase(Ease.InOutQuad);
            firstSlot.transform.DOMove(firstSkillSlot, skillSwapSpeed).SetEase(Ease.InOutQuad);
            isSwap = false;
            firstSlot.transform.SetAsLastSibling();
        }        
    }
}
