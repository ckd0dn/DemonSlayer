using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckPointUI : UIBase
{
    public PlayerStateMachine stateMachine;
    public Player player;
    public Button firstBtn;
    public Button secondBtn;
    //public Button thirdBtn;
    public Button fourthBtn;
    public TextMeshProUGUI currentCheckpointName;
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    public CheckPoint currentCheckpoint;
    MainMenuUI mainMenuUI;
    public RectTransform BtnGroup;

    private void Awake()
    {
       mainMenuUI = UIManager.Instance.mainMenuUI;
    }
    private void OnEnable()
    {
        stateMachine = GameManager.Instance.Player.stateMachine;
        player = GameManager.Instance.Player;
        GameManager.Instance.MonsterMove = false;

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

        if(currentCheckpoint != null)
        {
            currentCheckpoint.interactText.SetActive(true);
        }
        GameManager.Instance.MonsterMove = true;
    }
    public void BtnActive()
    {
        foreach (Button btn in BtnGroup.GetComponentsInChildren<Button>())
        {
            btn.interactable = true; 
        }
    }

    public void BtnUnActive()
    {
        foreach (Button btn in BtnGroup.GetComponentsInChildren<Button>())
        {
            btn.interactable = false;
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
        mainMenuUI.invenItemRelics.BtnActive = true;
        mainMenuUI.invenEquipRelics.BtnActive = true;
    }
}
