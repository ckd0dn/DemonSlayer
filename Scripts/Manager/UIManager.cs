using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    // ������ UI��
    // �� ���� ��� ã��
    public UI ui;    
    public MainMenuUI mainMenuUI;
    public PauseUI pauseUI;
    public CheckPointUI checkPointUI;
    public DieUI dieUI;
    public SoulUI soulUI;
    public SkillShopUI skillShopUI;
    public FastTravelUI fastTravelUI;
    public SkillEquipMenu skillEquipMenu;

    private Player player;
    public bool onUI = false; // UI Ȱ��ȭ ���� ����, �ʿ��Ѱ�??
    public UIBase activeUI = null; // ���� Ȱ��ȭ �� UI ����

    public delegate void OnUIEvent(bool state);
    public event OnUIEvent onUIEvent;
    
    private void Start()
    {        
        player = GameManager.Instance.Player;        
        player.Input.UIActions.MainMenu.started += context => ToggleUI(ref mainMenuUI, 1f ,1f, false, false, mainMenuUI.currentTapIndex);
        player.Input.UIActions.Pause.started += context => ToggleUI(ref pauseUI, 0f, 1f, true);
        player.Input.UIActions.Status.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 0);
        player.Input.UIActions.Skill.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 1);
        player.Input.UIActions.Item.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false,2);
        player.Input.UIActions.Map.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 3);        

        SettingGameScene();
    }

    public T Show<T>() where T : UIBase
    {
        var loadUI = ResourceManager.Instance.LoadUI<T>();
        return Instantiate(loadUI);
    }

    public GameObject SettingGameObj(string name)
    {
        var go = ResourceManager.Instance.LoadGameObject(name);
        return Instantiate(go);
    }

    public void SettingGameScene()
    {
        // SettingGameObj("Stage1");
        ui = Show<UI>();
        soulUI = Show<SoulUI>();
        mainMenuUI = Show<MainMenuUI>();
        mainMenuUI.gameObject.SetActive(false);
    }

    // ���� Toggle �Լ�
    public void ToggleUI<T>(ref T ui, float uiOnTimeScale = 1f, float uiOffTimeScale = 1f, bool isESC = false, bool isCheckpoint = false, int mainMenuIndex = 0) where T : UIBase
    {        
        // UI �ߺ� Ȱ��ȭ ����
        if (ui == null)
        {
            //onUI = true;
            SetOnUI(true);
            player.Input.PlayerActions.Disable();
            ui = Show<T>();
            ui.gameObject.SetActive(false);
        }

        // UI �ߺ� Ȱ��ȭ ����
        if (activeUI != null && activeUI != ui)
        {
            // UI Ȱ��ȭ �߿� ESC �Է� �� ��Ȱ��ȭ
            if (isESC)
            {
                activeUI.OffUI();
                //onUI = false;
                SetOnUI(false);
                player.Input.PlayerActions.Enable();
                activeUI = null;
                Time.timeScale = 1f;
                return;
            }
            if (isCheckpoint)
            {
                activeUI.OffUI();
                //onUI = false;
                SetOnUI(false);
                player.Input.PlayerActions.Enable();

                activeUI = null;
                Time.timeScale = 1f;
            }
        }

        // MainMenu ���� ó��
        if (ui is MainMenuUI mainMenuUI)
        {
            if (activeUI != ui && onUI)
            {
                return;
            }

            if (activeUI == ui)
            {
                if (mainMenuIndex != mainMenuUI.currentTapIndex)
                {
                    mainMenuUI.SwapMainMenu(mainMenuIndex);
                    return;
                }
            }
            else
            {
                mainMenuUI.SwapMainMenu(mainMenuIndex);
            }
        }

        if (ui.gameObject.activeSelf)
        {
            Time.timeScale = uiOffTimeScale;
            ui.OffUI();
            //onUI = false;
            SetOnUI(false);
            player.Input.PlayerActions.Enable();
            activeUI = null;
        }
        else
        {
            Time.timeScale = uiOnTimeScale;
            ui.OnUI();
            //onUI = true;
            SetOnUI(true);
            player.Input.PlayerActions.Disable();
            activeUI = ui;
        }
    }    

    public void SetOnUI(bool state)
    {
        onUI = state;
        onUIEvent?.Invoke(onUI);
    }
}