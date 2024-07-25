using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicsItemSlot : MonoBehaviour
{
    public ItemSO itemData;
    public int index;
    public Image itemicon;
    public Button ItemInfoBtn;
    StatHandler statHandler;
    public Stats itemEquipStat;

    public InvenItemRelics invenItemRelics;

    private void Start()
    {
        ItemInfoBtn = GetComponent<Button>();
        ItemInfoBtn.onClick.AddListener(OnItemClicked);
        statHandler = GameManager.Instance.Player.StatHandler;
    }
    public void StatChange()
    {
        statHandler.AddStatModifier(itemEquipStat);
    }
    public void SetItem(ItemSO newItem)
    {
        itemData = newItem;
        itemicon.color = new Color(20, 20, 20);
        itemicon.sprite = newItem.itemImage;
    }
    public bool IsEmpty()
    {
        return itemData == null;
    }
    private void OnItemClicked()
    {
        invenItemRelics.SelectItemIndex = index;
        invenItemRelics.UpdateItemUI();
    }
}
