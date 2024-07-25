using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    PauseUI pasueUI;


    public void TogglePaseUI()
    {
        if (pasueUI == null)
        {
            pasueUI = UIManager.Instance.Show<PauseUI>();
        }
        else
        {
            if (pasueUI.gameObject.activeSelf)
            {
                Time.timeScale = 1f;
                pasueUI.OffUI();
            }
            else
            {
                Time.timeScale = 0f;
                pasueUI.OnUI();
            }
        }
    }
}
