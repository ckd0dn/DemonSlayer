using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointUI : UIBase
{
    public PlayerStateMachine stateMachine;
    public Player player;
    public Button firstBtn;
    public Button secondBtn;
    //public Button thirdBtn;
    public Button fourthBtn;
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    private void OnEnable()
    {
        stateMachine = GameManager.Instance.Player.stateMachine;
        player = GameManager.Instance.Player;
        firstBtn.Select();
        if (CinemachineVirtualCamera != null)
        {
            CinemachineVirtualCamera.Priority = 20;
            stateMachine.ChangeState(stateMachine.RestState);
        }
        else
        {
            CinemachineVirtualCamera = GameManager.Instance.Player.GetComponentInChildren<CinemachineVirtualCamera>();
            CinemachineVirtualCamera.Priority = 20;         
            stateMachine.ChangeState(stateMachine.RestState);
        }

    }

    private void Start()
    {
        CinemachineVirtualCamera = GameManager.Instance.Player.GetComponentInChildren<CinemachineVirtualCamera>();       
    }


    private void OnDisable()
    {
        if (CinemachineVirtualCamera != null)
        {
            CinemachineVirtualCamera.Priority = 0;
        }       
    }

    public void OnClickFastTravelBtn()
    {
        UIManager.Instance.ToggleUI(ref UIManager.Instance.fastTravelUI, 0f, 1f, false, true);
    }

    public void OnSecondBtn()
    {
        UIManager.Instance.ToggleUI(ref UIManager.Instance.skillShopUI, 0f, 1f, false, true);
        UIManager.Instance.skillShopUI.CanBuySkillColor();
    }

    public void OnThirdBtn()
    {
        UIManager.Instance.ToggleUI(ref UIManager.Instance.skillEquipMenu, 0f, 1f, false, true);
    }

    public void OnClickfourthBtn()
    {
        UIManager.Instance.ToggleUI(ref UIManager.Instance.mainMenuUI, 0f, 1f, false, true);
        UIManager.Instance.mainMenuUI.SwapMainMenu(2);
        UIManager.Instance.mainMenuUI.invenItemRelics.BtnActive = true;
        UIManager.Instance.mainMenuUI.invenEquipRelics.BtnActive = true;
    }

}
