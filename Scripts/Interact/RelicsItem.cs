using UnityEngine;

public class RelicsItem : InteractableObject
{
    public ItemSO itemSO;

    public override void Interact()
    {
        AddInventory();
        player.Input.PlayerActions.Interaction.started -= OnInteract;
    }
    private void AddInventory()
    {
        if (GameManager.Instance != null)
        {
            UIManager.Instance.mainMenuUI.AddItemToInventory(itemSO);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("¥Í¿∫¿Ã∏ß" + collision.gameObject.name);
    }
}
