using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicsEquipSlot : MonoBehaviour
{
    public ItemSO EquipitemData;
    public int index;
    public Image itemicon;
    public Button itemInfoBtn;
    StatHandler statHandler;

    public InvenEquipRelics invenEquipRelics;
    private void Start()
    {
        itemInfoBtn = GetComponent<Button>();
        itemInfoBtn.onClick.AddListener(OnItemClicked);
        statHandler = GameManager.Instance.Player.StatHandler;
    }
    public void StatChange(int index)
    {
        statHandler.RemoveStatModifier(index);
    }
    public void SetItem(ItemSO newItem)
    {
        EquipitemData = newItem;
        itemicon.color = new Color(20, 20, 20);
        itemicon.sprite = newItem.itemImage;
    }
    public bool IsEmpty()
    {
        return EquipitemData == null;
    }
    private void OnItemClicked()
    {
        invenEquipRelics.Unequipindex = index;
        invenEquipRelics.UpdateItemUI();
    }

}
