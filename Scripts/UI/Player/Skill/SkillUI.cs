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
    private Color TransparencyColor = new Color(1, 1, 1, 0);
    private Color colored = new Color(1, 1, 1, 1);

    private void Awake()
    {
        UIManager.Instance.skillUI = this;
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        image = GetComponent<Image>();
        icon = image.sprite;
        skillList = player.playerEquipSkill;
        firstSkillSlot = firstSlot.transform.position;
        secondSkillSlot = secondSlot.transform.position;
        StartSkillIcon(0f);

        OnSkillUpdate();
    }

    public void StartSkillIcon(float alpha)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            Color color = skills[i].color;
            color.a = Mathf.Clamp01(alpha);
            skills[i].color = color;
        }
    }

    public void OnSkillUpdate()
    {
        skillList = player.playerEquipSkill;
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i] == null)
            {
                skills[i].sprite = null;
                skills[i].color = TransparencyColor;
            }
            else
            {
                skills[i].GetComponent<Image>().sprite = skillList[i].SkillIcon;
                skills[i].color = colored;
            }
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
