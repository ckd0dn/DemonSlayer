using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UIBase
{
    public int currentTapIndex = 0;

    public Button statusBtn;
    public Button skillBtn;
    public Button itemBtn;
    public Button mapBtn;

    public GameObject statusPanel;
    public GameObject skillPanel;
    public GameObject itemPanel;
    public GameObject mapPanel;

    public InvenItemRelics invenItemRelics;
    public InvenEquipRelics invenEquipRelics;
    public InvenPassiveEquip invenPassiveEquip;

    public void SwapMainMenu(int index)
    {
        switch (index)
        {
            case 0:
                statusPanel.transform.SetAsLastSibling();
                currentTapIndex = 0;
                break;
            case 1:
                skillPanel.transform.SetAsLastSibling();
                currentTapIndex = 1;
                break;
            case 2:
                itemPanel.transform.SetAsLastSibling();
                currentTapIndex = 2;
                break;
            case 3:
                mapPanel.transform.SetAsLastSibling();
                currentTapIndex = 3;
                break;
        }
    }

    public bool AddItemToInventory(ItemSO item)
    {
        if(item == null)
        {
            return false;
        }

        if (invenItemRelics != null)
        {
            return invenItemRelics.AddItemToSlot(item);
        }

        DataManager.Instance.SaveData();

        return false;
    }

    public bool AddPassiveItem(ItemSO item) 
    {
        if(invenPassiveEquip != null)
        {
            return invenPassiveEquip.AddItemToSlot(item);
        }

        DataManager.Instance.SaveData();

        return false;
    }
}
