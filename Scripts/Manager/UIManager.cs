using System.Collections;
using System.Collections.Generic;
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
    public BossUI bossUI;
    public SkillUI skillUI;
    public GetItemUI getItemUI;
    public WorldCanvas worldCanvas;
    public TutorialUI tutorialUI;   
    public BuffIcon buffIcon;

    private Player player;
    public bool onUI = false; // UI Ȱ��ȭ ���� ����, �ʿ��Ѱ�??
    public UIBase activeUI = null; // ���� Ȱ��ȭ �� UI ����

    public delegate void OnUIEvent(bool state);
    public event OnUIEvent onUIEvent;
    protected override void Awake()
    {
        base.Awake();
        // mainMenuUI가 null이면 초기화 start에서 진행하면 인터렉터블오브젝트에서 null이 발생함 
        if (mainMenuUI == null)
        {
            mainMenuUI = Show<MainMenuUI>();
            mainMenuUI.gameObject.SetActive(false);
        }
        if(pauseUI == null)
        {
            pauseUI = Show<PauseUI>();
            pauseUI.gameObject.SetActive(false);    
        }
        SettingGameScene();
    }
    private void Start()
    {        
        player = GameManager.Instance.Player;        
        player.Input.UIActions.MainMenu.started += context => ToggleUI(ref mainMenuUI, 1f ,1f, false, false, mainMenuUI.currentTapIndex);
        player.Input.UIActions.Pause.started += context => ToggleUI(ref pauseUI, 0f, 1f, true);
        player.Input.UIActions.Status.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 0);
        player.Input.UIActions.Skill.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 1);
        player.Input.UIActions.Item.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false,2);
        player.Input.UIActions.Map.started += context => ToggleUI(ref mainMenuUI, 1f, 1f, false, false, 3);        
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
        worldCanvas = Show<WorldCanvas>();
        ui = Show<UI>();
        soulUI = Show<SoulUI>();
        bossUI = Show<BossUI>();
        buffIcon = Show<BuffIcon>();
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