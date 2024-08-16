using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : MonoBehaviour
{
    public ItemSO itemData;
    public int index;
    public Image itemicon;
    public Button ItemInfoBtn;
    Player player;
    Image[] images;
    public InvenPassiveEquip InvenPassiveEquip;
    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
        player = GameManager.Instance.Player;
        itemicon = images[1];
        ItemInfoBtn = GetComponent<Button>();
        ItemInfoBtn.onClick.AddListener(OnItemClicked);
    }
    public void SetItem(ItemSO newItem)
    {
        itemData = newItem;
        itemicon.color = new Color(20, 20, 20);
        itemicon.sprite = newItem.itemImage;

        if(itemData.playerActive == PlayerActive.DoubleJump)
        {
            player.DoubleJumpGet = true;
        }

        if(itemData.playerActive == PlayerActive.Dash)
        {
            player.DashGet = true;
        }
        
    }
    public bool IsEmpty()
    {
        return itemData == null;
    }

    private void OnItemClicked()
    {
        InvenPassiveEquip.SelectItemIndex = index;
        InvenPassiveEquip.UpdateUI();
    }
}
