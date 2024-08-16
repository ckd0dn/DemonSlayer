using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    public Animator animator;
    public Image image;

    public void Show()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Save");

        Invoke("Hide", 1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
