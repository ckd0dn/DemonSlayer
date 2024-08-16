using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvenPassiveEquip : MonoBehaviour
{
    public PassiveSlot[] passiveSlots;
    StatHandler statHandler;
    public Stats UpdateStats;
    public int SelectItemIndex;

    public TextMeshProUGUI PassiveItemDescription;
    private void Awake()
    {
        statHandler = GameManager.Instance.Player.StatHandler;
        passiveSlots = GetComponentsInChildren<PassiveSlot>();
        UpdateSlotIndex();
        
    }
    private void OnEnable()
    {
        PassiveItemDescription.text = string.Empty;
    }
    private void UpdateSlotIndex()
    {
        for (int i = 0; i < passiveSlots.Length; i++)
        {
            passiveSlots[i].index = i;
            passiveSlots[i].InvenPassiveEquip = this;
        }
    }

    public bool AddItemToSlot(ItemSO item)
    {
        for (int i = 0; i < passiveSlots.Length; i++)   
        {
            if (passiveSlots[i].IsEmpty())
            {
                passiveSlots[i].SetItem(item);
                UpdateStats.statsSO = passiveSlots[i].itemData;
                CharacterUpdateState();
                return true;
            }
        }
        return true;
    }

    public void CharacterUpdateState()
    {
        statHandler.AddStatModifier(UpdateStats);
    }

    public void UpdateUI()
    {
        PassiveSlot selectIndex = passiveSlots[SelectItemIndex];
        if (selectIndex.itemData != null)
        {
            PassiveItemDescription.text = selectIndex.itemData.description;
        }
        else
        {
            PassiveItemDescription.text = string.Empty;
        }
    }

}
