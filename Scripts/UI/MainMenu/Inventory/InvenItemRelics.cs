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
        UpdateSlotIndex();
    }
    private void OnEnable()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        EquipBtn.SetActive(false);
        UnEquipBtn.SetActive(false);
        BtnActive = false;
    }
    private void UpdateSlotIndex()
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
            // selectIndex.itemEquipStat.statsSO = selectIndex.itemData;
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
            if (BtnActive)
            {
                EquipBtn.SetActive(false);
            }
        }
    }
    public void ItemEquip()
    {
        RelicsItemSlot SelectIndex = relicsItemSlots[SelectItemIndex];
        //UIManager.Instance.mainMenuUI.invenEquipRelics.AddEquipItemToSlot(SelectIndex.itemData);
        if (UIManager.Instance.mainMenuUI.invenEquipRelics.AddEquipItemToSlot(SelectIndex.itemData) == true)
        {
            SelectIndex.StatChange();
            EquipBtn.SetActive(false);
            SelectIndex.itemData = null;
            SelectIndex.itemicon.sprite = null;
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;


            for (int i = SelectItemIndex; i < relicsItemSlots.Length - 1; i++)
            {
                relicsItemSlots[i].itemData = relicsItemSlots[i + 1].itemData;
                relicsItemSlots[i].itemicon.sprite = relicsItemSlots[i + 1].itemicon.sprite;
                if (relicsItemSlots[i].itemData == null)
                {
                    relicsItemSlots[i].itemicon.color = new Color32(20, 20, 20, 255);
                }
            }

            int lastIndex = relicsItemSlots.Length - 1;
            relicsItemSlots[lastIndex].itemData = null;
            relicsItemSlots[lastIndex].itemicon.sprite = null;
            relicsItemSlots[lastIndex].itemicon.color = new Color32(20, 20, 20, 255);
        }

        DataManager.Instance.SaveData();
    }


}
