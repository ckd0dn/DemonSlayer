using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenEquipRelics : MonoBehaviour
{
    public Transform SlotHolder;
    public RelicsEquipSlot[] relicsEquipSlots;
    public int Unequipindex;

    //유물 정보 UI 세팅 
    [Header("UI Setting")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public GameObject unEquipBtn;
    public GameObject EquipBtn;

    public bool BtnActive;
    private void Awake()
    {
        relicsEquipSlots = GetComponentsInChildren<RelicsEquipSlot>();
        for (int i = 0; i < relicsEquipSlots.Length; i++)
        {
            relicsEquipSlots[i].index = i;
            relicsEquipSlots[i].invenEquipRelics = this;
        }
    }
    
    public bool AddEquipItemToSlot(ItemSO item)
    {
        for (int i = 0; i < relicsEquipSlots.Length; i++)
        {
            if (relicsEquipSlots[i].IsEmpty())
            {
                relicsEquipSlots[i].SetItem(item);
                return true;
            }
        }
        return false;
    }

    public void ItemUnEquip()
    {
        RelicsEquipSlot Selectindex = relicsEquipSlots[Unequipindex];
        UIManager.Instance.mainMenuUI.invenItemRelics.AddItemToSlot(Selectindex.EquipitemData);
        Selectindex.StatChange(Selectindex.invenEquipRelics.Unequipindex);

        //UI전부 끄기
        unEquipBtn.SetActive(false);
        Selectindex.EquipitemData = null;
        Selectindex.itemicon.sprite = null;
        Selectindex.itemicon.color = new Color(20,20,20);
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;

        for (int i = Unequipindex; i < relicsEquipSlots.Length - 1; i++)
        {
            relicsEquipSlots[i].EquipitemData = relicsEquipSlots[i + 1].EquipitemData;
            relicsEquipSlots[i].itemicon.sprite = relicsEquipSlots[i + 1].itemicon.sprite;
            relicsEquipSlots[i].itemicon.color = relicsEquipSlots[i + 1].itemicon.color;
        }

        // 마지막 슬롯 초기화
        int lastIndex = relicsEquipSlots.Length - 1;
        relicsEquipSlots[lastIndex].EquipitemData = null;
        relicsEquipSlots[lastIndex].itemicon.sprite = null;
        relicsEquipSlots[lastIndex].itemicon.color = new Color(20, 20, 20);
    }

    public void UpdateItemUI()
    {
        RelicsEquipSlot Selectindex = relicsEquipSlots[Unequipindex];
        if (EquipBtn.activeSelf)
        {
            EquipBtn.SetActive(false);
        }
        if (Selectindex.EquipitemData != null)
        {
            itemName.text = Selectindex.EquipitemData.itemName;
            itemDescription.text = Selectindex.EquipitemData.description;
            if (BtnActive)
            {
                unEquipBtn.SetActive(true);
            }
        }
        else
        {
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
        }
    }

}
