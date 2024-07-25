using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    protected Player player;
    [SerializeField] protected GameObject interactText;
    [SerializeField] protected string InteractTextDescription;
    MainMenuUI mainMenuUI;
    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
        mainMenuUI = UIManager.Instance.mainMenuUI;
        if (interactText != null)
        {
            interactText.GetComponentInChildren<TextMeshProUGUI>().text = InteractTextDescription;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Input.PlayerActions.Interaction.started += OnInteract;
            mainMenuUI.invenItemRelics.BtnActive = true;
            mainMenuUI.invenEquipRelics.BtnActive = true;
            ItemUIUpdate();
            ShowInteractText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            player.Input.PlayerActions.Interaction.started -= OnInteract;
            mainMenuUI.invenItemRelics.BtnActive = false;
            mainMenuUI.invenEquipRelics.BtnActive = false;
            ItemUIUpdate();
            HideInteractText();
        }

    }

    protected void OnInteract(InputAction.CallbackContext context)
    {
        Interact();
    }

    public abstract void Interact();

    protected void ShowInteractText()
    {
        if(interactText != null)
        {
            interactText.SetActive(true);
        }
    }

    protected void HideInteractText()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    private void ItemUIUpdate()
    {
        mainMenuUI.invenItemRelics.UpdateItemUI();
        mainMenuUI.invenEquipRelics.UpdateItemUI();
    }
}
