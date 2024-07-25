using TMPro;
using UnityEngine;

public class InvenItemRelics : MonoBehaviour
{
    public Transform SlotHolder;
    public RelicsItemSlot[] relicsItemSlots;
    public GameObject RelicsItemSlotPrefab;
    public int SelectItemIndex;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public GameObject EquipBtn;
    public GameObject UnEquipBtn;

    public bool BtnActive;
    private void Awake()
    {
        relicsItemSlots = GetComponentsInChildren<RelicsItemSlot>();
        UpdateSlotIndices();
    }

    private void UpdateSlotIndices()
    {
        for (int i = 0; i < relicsItemSlots.Length; i++)
        {
            relicsItemSlots[i].index = i;
            relicsItemSlots[i].invenItemRelics = this;
        }
    }

    public bool AddItemToSlot(ItemSO item)
    {
        for (int i = 0; i < relicsItemSlots.Length; i++)
        {
            if (relicsItemSlots[i].IsEmpty())
            {
                relicsItemSlots[i].SetItem(item);
                return true;
            }
        }

        CreateNewSlot().SetItem(item);
        return true;
    }

    private RelicsItemSlot CreateNewSlot()
    {
        GameObject createSlot = Instantiate(RelicsItemSlotPrefab, SlotHolder);
        RelicsItemSlot createRelicsItemSlot = createSlot.GetComponent<RelicsItemSlot>();

        var tempList = new System.Collections.Generic.List<RelicsItemSlot>(relicsItemSlots);
        tempList.Add(createRelicsItemSlot);
        relicsItemSlots = tempList.ToArray();

        createRelicsItemSlot.index = relicsItemSlots.Length - 1;
        return createRelicsItemSlot;
    }

    public void UpdateItemUI()
    {
        RelicsItemSlot selectIndex = relicsItemSlots[SelectItemIndex];
        if (UnEquipBtn.activeSelf)
        {
            UnEquipBtn.SetActive(false);
        }

        if (selectIndex.itemData != null)
        {
            selectIndex.itemEquipStat.statsSO = selectIndex.itemData;
            itemName.text = selectIndex.itemData.itemName;
            itemDescription.text = selectIndex.itemData.description;
            if (BtnActive)
            {
                EquipBtn.SetActive(true);
            }

        }
        else
        {
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
        }
    }
    public void ItemEquip()
    {
        RelicsItemSlot SelectIndex = relicsItemSlots[SelectItemIndex];
        UIManager.Instance.mainMenuUI.invenEquipRelics.AddEquipItemToSlot(SelectIndex.itemData);
        SelectIndex.StatChange();
        EquipBtn.SetActive(false);
        SelectIndex.itemData = null;
        SelectIndex.itemicon.sprite = null;
        SelectIndex.itemicon.color = new Color(20, 20, 20);
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;


        for (int i = SelectItemIndex; i < relicsItemSlots.Length - 1; i++)
        {
            relicsItemSlots[i].itemData = relicsItemSlots[i + 1].itemData;
            relicsItemSlots[i].itemicon.sprite = relicsItemSlots[i + 1].itemicon.sprite;
            relicsItemSlots[i].itemicon.color = relicsItemSlots[i + 1].itemicon.color;
        }

        // 마지막 슬롯 초기화
        int lastIndex = relicsItemSlots.Length - 1;
        relicsItemSlots[lastIndex].itemData = null;
        relicsItemSlots[lastIndex].itemicon.sprite = null;
        relicsItemSlots[lastIndex].itemicon.color = new Color(20, 20, 20);

    }


}
