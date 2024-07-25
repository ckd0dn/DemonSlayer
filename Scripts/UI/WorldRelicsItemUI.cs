using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldRelicsItemUI : InteractableObject
{
    public GameObject itemDescription;
    public Image ItemIcon;
    public TextMeshProUGUI ItemName;

    ItemSO itemSO; 
    RelicsItem relicsItem;

    protected override void Start()
    {
        base.Start();
        relicsItem = GetComponent<RelicsItem>();
        if (relicsItem != null)
        {
            itemSO = relicsItem.itemSO;
        }
    }

    public override void Interact()
    {
        ItemDescription();
    }

    private void ItemDescription()
    {
        itemDescription.SetActive(true);

        ItemIcon.sprite = itemSO.itemImage;
        ItemName.text = itemSO.itemName;
        StartCoroutine(FalseItemUI());
    }

    IEnumerator FalseItemUI()
    {
        yield return new WaitForSecondsRealtime(1f);
        itemDescription.SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }
}
