using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : UIBase
{
    public Image ItemIcon;
    public TextMeshProUGUI ItemName;

    ItemSO itemSO; 

    public void SetItemSO(ItemSO newItemSO)
    {
        itemSO = newItemSO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (itemSO != null)
        {
            ItemIcon.sprite = itemSO.itemImage; 
            ItemName.text = itemSO.itemName; 
        }
        if(gameObject.activeSelf) 
        StartCoroutine(FalseItemUI());
    }

    IEnumerator FalseItemUI()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
        yield return null;
    }
}
