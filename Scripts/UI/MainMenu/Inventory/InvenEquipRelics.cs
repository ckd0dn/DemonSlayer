using System.Collections;
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
    public GameObject equipBtn;
    public GameObject noHaveSlotMessage;
    public bool BtnActive;

    private void Awake()
    {
        relicsEquipSlots = GetComponentsInChildren<RelicsEquipSlot>();
        for (int i = 0; i < relicsEquipSlots.Length; i++)
        {
            relicsEquipSlots[i].index = i;
            relicsEquipSlots[i].invenEquipRelics = this;
        }
        noHaveSlotMessage.SetActive(false);
    }

    private void OnEnable()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        unEquipBtn.SetActive(false);
        equipBtn.SetActive(false);
        BtnActive = false;
    }

    private void OnDisable()
    {
        noHaveSlotMessage.SetActive(false);
    }

    public bool AddEquipItemToSlot(ItemSO item)
    {
        if (item == null)
        {
            return false;
        }
        for (int i = 0; i < relicsEquipSlots.Length; i++)
        {
            if (relicsEquipSlots[i].IsEmpty())
            {
                relicsEquipSlots[i].SetItem(item);
                return true;
            }
        }
        StartCoroutine(NoHaveSlotMessage());
        return false;
    }

    private IEnumerator NoHaveSlotMessage()
    {
        noHaveSlotMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        noHaveSlotMessage.SetActive(false);
    }

    public void ItemUnEquip()
    {
        RelicsEquipSlot Selectindex = relicsEquipSlots[Unequipindex];
        UIManager.Instance.mainMenuUI.invenItemRelics.AddItemToSlot(Selectindex.EquipitemData);
        Selectindex.RemoveStat(Selectindex.invenEquipRelics.Unequipindex);

        //UI전부 끄기
        unEquipBtn.SetActive(false);
        Selectindex.EquipitemData = null;
        Selectindex.itemicon.sprite = null;
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;

        for (int i = Unequipindex; i < relicsEquipSlots.Length - 1; i++)
        {
            relicsEquipSlots[i].EquipitemData = relicsEquipSlots[i + 1].EquipitemData;
            relicsEquipSlots[i].itemicon.sprite = relicsEquipSlots[i + 1].itemicon.sprite;
            if (relicsEquipSlots[i].EquipitemData == null)
            {
                relicsEquipSlots[i].itemicon.color = new Color32(20, 20, 20, 255);
            }
        }

        // 마지막 슬롯 초기화
        int lastIndex = relicsEquipSlots.Length - 1;
        relicsEquipSlots[lastIndex].EquipitemData = null;
        relicsEquipSlots[lastIndex].itemicon.sprite = null;
        relicsEquipSlots[lastIndex].itemicon.color = new Color32(20, 20, 20, 255);

        DataManager.Instance.SaveData();
    }

    public void UpdateItemUI()
    {
        RelicsEquipSlot Selectindex = relicsEquipSlots[Unequipindex];
        if (equipBtn.activeSelf)
        {
            equipBtn.SetActive(false);
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
            if (BtnActive)
            {
                unEquipBtn.SetActive(false);
            }
        }
    }

}
