using System;
using System.Collections;
using UnityEngine;

public class RelicsItem : InteractableObject
{
    public ItemSO itemSO;
    GetItemUI getItemUI;
    public bool isGet = false;

    protected override void Start()
    {
        base.Start();
        getItemUI = UIManager.Instance.getItemUI;
    }

    public override void Interact()
    {
        AddInventory();
        UIManager.Instance.ToggleUI(ref getItemUI, 1f, 1f, false, false);

        if (getItemUI != null)
        {
            getItemUI.SetItemSO(itemSO);
        }
        player.Input.PlayerActions.Interaction.started -= OnInteract;
        StartCoroutine(FalseItem());
    }

    IEnumerator FalseItem()
    {
        UIManager.Instance.SetOnUI(false);
        player.Input.PlayerActions.Enable();
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }

    private void AddInventory()
    {
        if (GameManager.Instance != null)
        {
            if (itemSO.itemType == ItemType.Equip)
            {
                UIManager.Instance.mainMenuUI.AddItemToInventory(itemSO);

            }
            else
            {
                UIManager.Instance.mainMenuUI.AddPassiveItem(itemSO);
            }
        }

        //æ∆¿Ã≈€ ∏‘¿Ω
        isGet = true;
        DataManager.Instance.SaveData();
    }
}

