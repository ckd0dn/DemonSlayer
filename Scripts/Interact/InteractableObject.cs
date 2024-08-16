using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour
{
    protected Player player;
    public GameObject interactText;
    [SerializeField] protected string InteractTextDescription;
    protected bool canInteract = true;

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
        if (interactText != null)
        {
            interactText.GetComponentInChildren<TextMeshProUGUI>().text = InteractTextDescription;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canInteract)
        {
            player.Input.PlayerActions.Interaction.started += OnInteract;
            ShowInteractText();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canInteract) 
        {
            player.Input.PlayerActions.Interaction.started -= OnInteract;
            HideInteractText();
        }

    }

    protected void OnInteract(InputAction.CallbackContext context)
    {
        if(canInteract)
        {
            Interact();
        }
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


}
