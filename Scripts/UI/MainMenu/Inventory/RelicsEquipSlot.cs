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
    public void AddStat()
    {
        statHandler = GameManager.Instance.Player.StatHandler;
        Stats stats = new Stats();
        stats.statsSO = EquipitemData;
        statHandler.AddStatModifier(stats);
    }
    public void RemoveStat(int index)
    {
        statHandler.RemoveStatModifier(index);
    }
    public void SetItem(ItemSO newItem)
    {
        EquipitemData = newItem;
        itemicon.color = Color.white;
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
