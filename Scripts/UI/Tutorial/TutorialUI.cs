using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIBase
{
    public GameObject whiteImg;
    private Image whiteImage;
    public Text text;
    public GameObject TextPanel;
    public Transform moveTutorialTarget;
    public TextMeshProUGUI skipText;

    // 튜토리얼 텍스트 그룹
    private string[][] tutorialTexts;
    private WaitForSeconds waitTextDelay = new WaitForSeconds(4f);
    private WaitForSeconds Delay = new WaitForSeconds(2f);
    Player player;
    // UI 설정
    public CheckPointUI checkPointUI;
    [SerializeField] PauseUI pauseUI;
    [SerializeField] MainMenuUI mainMenuUI;
    public BoxCollider2D ChackpointCollider;
    public BoxCollider2D TutorialCollider;
    public BoxCollider2D MoveCollider;
    public PracticeMonster ScareCrow;
    public GameObject SkipBtn;
    public Button firstBtn;

    private void Awake()
    {
        whiteImage = whiteImg.GetComponent<Image>();
        ChackpointCollider.enabled = true;
        UIManager.Instance.tutorialUI = this;
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        pauseUI = UIManager.Instance.pauseUI;
        mainMenuUI = UIManager.Instance.mainMenuUI;
        player.Input.TutorialActions.Enable();
        // 튜토리얼 텍스트 초기화
        tutorialTexts = new string[][]
        {
            new string[] // 움직임
            {
                "좌우 방향키로 움직일 수 있습니다.",
                "앞에 빛나는 곳으로 이동해보세요",
                "움직임이 좋으시네요."
            },
            new string[] // 체크포인트
            {
                "정면에 있는 석상은 체크포인트 입니다.",
                "석상에 다가가서 'E' 를 눌러보세요.",
                "체크포인트를 상호작용하면 게임이 저장이 됩니다.",
                "체크포인트 에서는 다양한 상호작용을 할 수 있습니다.",
                "빠른 이동은, 체크포인트를 저장한 곳으로 순간이동 할 수 있습니다.",
                "제단은 몬스터를 잡아 소울을 획득해서, 스킬을 구매할 수 있습니다.",
                "스킬은 제단에서 구매한 스킬을 장착 할 수 있습니다.",
                "스킬 사용 단축키는 'A','S','D' 입니다. 'F'키로 스킬 슬롯 전환이 가능합니다.",
                "아이템은 맵 곳곳에 숨어있는 아이템 혹은 보스를 잡아서 획득하는 아이템을 장착 할 수 있습니다.",
                "ESC 버튼을 눌러 창을 닫을 수 있습니다."
            },
            new string[] // 설정
            {
                "ESC 버튼을 눌러 설정 창을 열 수 있습니다.",
                "계속 버튼을 눌러 게임으로 돌아 갈 수 있습니다.",
                "체크포인트로 돌아가기는, 이전에 저장해뒀던 체크포인트로 이동할 수 있습니다.",
                "설정 에서는 배경음과 해상도를 조절 할 수 있습니다.",
                "타이틀로 돌아가기는 시작화면으로 돌아갑니다.",
                "타이틀로 돌아가시기 전에 꼭 체크포인트 저장을 하고 진행하시길 바랍니다.",
                "게임종료 버튼입니다. 종료하시기전에 체크포인트 저장을 하고 종료하시길 바랍니다.",
                "ESC 버튼을 눌러 창을 닫을 수 있습니다."
            },
            new string[] // 캐릭터
            {
                "TAB키를 눌러 캐릭터의 정보를 볼 수 있습니다.",
                "TAB키를 눌러 보세요",
                "상태에서 캐릭터의 정보를 확인 할 수 있습니다. 'P'키를 사용해도 확인 할 수 있습니다.",
                "아래의 네모칸은 패시브 아이템 슬롯으로 아이템을 획득하면 자동으로 장착이 됩니다.",
                "스킬에서 스킬의 설명을 보실 수 있습니다. 장착은 체크포인트 에서만 가능합니다.'K'키를 사용해도 확인 할 수 있습니다.",
                "유물에서는 획득한 아이템을 확인하실 수 있습니다. 장착은 체크포인트 에서만 가능합니다. 'I'키를 사용해도 확인 할 수 있습니다.",
                "지도에서는 탐험한 곳을 확인할 수 있습니다. 'M'키를 사용해도 확인 할 수 있습니다.",
                "ESC 버튼을 눌러 창을 닫을 수 있습니다."
            },
            new string[] // 전투
            {
                "정면에 있는 허수아비를 공격해보세요. 공격은 'X'키 입니다.",
                "좋습니다. 공격은 총 3번까지 사용이 가능하며, 두 번째,세 번째 공격은 더 높은 데미지를 줄 수 있습니다.",
                "'Z'키로 구르기를 사용 할 수 있습니다. 구르기를 사용하는 중에는 몬스터의 공격을 받지 않습니다.",
                "'C'키로 점프를 할 수 있습니다. 높은 곳을 올라갈 때 사용해보세요",
                "기본적인 조작법은 여기까지 입니다. 진행하시느라 고생하셨습니다.",
                "당신의 모험에 행운이 깃들기를 바랍니다."
            }
        };

        Execute();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipBtn.SetActive(true);
            firstBtn.Select();
        }
    }

    public void Execute()
    {
        Datas datas = DataManager.Instance.GetData();

        if (datas.isPlayIntro)
        {
            ChackpointCollider.gameObject.SetActive(false);
            TutorialCollider.gameObject.SetActive(false);
            MoveCollider.gameObject.SetActive(false);
            player.Input.TutorialActions.Disable();
            PlayerInputEnable();
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(RunTutorial());
        }
    }

    public void SkipTutorial()
    {
        ChackpointCollider.gameObject.SetActive(false);
        TutorialCollider.gameObject.SetActive(false);
        MoveCollider.gameObject.SetActive(false); ;
        StopCoroutine(RunTutorial());
        player.Input.TutorialActions.Disable();
        PlayerInputEnable();
        gameObject.SetActive(false);
    }

    public void CloseSkip()
    {
        SkipBtn.SetActive(false);
    }

    void PlayerInputEnable()
    {
        player.Input.PlayerActions.Enable();
        player.Input.UIActions.Enable();
        TextPanel.SetActive(false);
    }
    void PlayerInputDisable()
    {
        player.Input.PlayerActions.Disable();
        player.Input.UIActions.Disable();
        TextPanel.SetActive(true);
    }
    IEnumerator RunTutorial()
    {
        yield return StartCoroutine(RunMovementTutorial());
        yield return StartCoroutine(RunCheckpointTutorial());
        yield return StartCoroutine(RunPauseTutorial());
        yield return StartCoroutine(RunCharacterTutorial());
        yield return StartCoroutine(RunBattleTutorial());
        player.Input.TutorialActions.Disable();
        PlayerInputEnable();
        gameObject.SetActive(false);
    }

    // 1. 움직임 튜토리얼
    IEnumerator RunMovementTutorial()
    {
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "좌,우 방향키로 움직일 수 있습니다.",
            "정면에 빛나는 곳으로 이동해보세요. "
        }));
        PlayerInputEnable();

        // 조건: 플레이어가 특정 위치에 도달할 때

        yield return new WaitUntil(() => PlayerHasReachedTargetPosition());
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "움직임이 좋으시네요."
        }));
    }

    // 2. 체크포인트 튜토리얼
    IEnumerator RunCheckpointTutorial()
    {
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "정면에 있는 석상은 체크포인트 입니다.",
            "석상에 다가가서 'E' 를 눌러보세요."
        }));
        PlayerInputEnable();

        // 조건: 체크포인트 UI 활성화 대기
        yield return new WaitUntil(() => CheckpointOn());
        PlayerInputDisable();
        checkPointUI.BtnUnActive();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "체크포인트를 상호작용하면 게임이 저장이 됩니다.",
            "체크포인트에서는 다양한 상호작용을 할 수 있습니다.",
        }));
        checkPointUI.BtnActive();
        checkPointUI.OnClickFastTravelBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "빠른 이동은, 체크포인트를 저장한 곳으로 순간이동 할 수 있습니다."

        }));
        checkPointUI.OnSecondBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "제단은 몬스터를 잡아 소울을 획득해서, 스킬을 구매할 수 있습니다.",
        }));
        checkPointUI.OnThirdBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "스킬은 제단에서 구매한 스킬을 장착 할 수 있습니다.",
            "스킬 사용 단축키는 'A','S','D' 입니다. 'F'키로 스킬 슬롯 전환이 가능합니다.",
        }));
        checkPointUI.OnClickfourthBtn();
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "아이템은 맵 곳곳에 숨어있는 아이템 혹은 보스를 잡아서 획득하는 아이템을 장착 할 수 있습니다.",
            "ESC 버튼을 눌러 창을 닫을 수 있습니다."
        }));
        mainMenuUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
        ChackpointCollider.enabled = false;
        PlayerInputEnable();
    }

    bool CheckpointOn()
    {
        checkPointUI = GameManager.Instance.TutorialCheckPoint;
        bool CheckpointOn = false;

        if (checkPointUI != null)
        {
            if (checkPointUI.gameObject.activeInHierarchy)
            {
                CheckpointOn = true;
            }
        }
        return CheckpointOn;
    }

    // 3. 설정 튜토리얼
    IEnumerator RunPauseTutorial()
    {
        yield return Delay;
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
                "ESC 버튼을 눌러 설정 창을 열 수 있습니다.",
                "ESC 버튼을 눌러보세요."
        }));

        TextPanel.SetActive(false);
        player.Input.PlayerActions.Disable();
        player.Input.UIActions.Enable();

        // 조건: ESC 키 입력 및 설정창 열림
        yield return new WaitUntil(() => pauseUI.gameObject.activeInHierarchy);
        pauseUI.BtnUnActive();
        Time.timeScale = 1f;
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "계속 버튼을 눌러 게임으로 돌아 갈 수 있습니다.",
            "체크포인트로 돌아가기는, 이전에 저장해뒀던 체크포인트로 이동할 수 있습니다.",
            "설정 에서는 사운드와 해상도를 조절 할 수 있습니다.",
            "타이틀로 돌아가기는 시작화면으로 돌아갑니다.",
            "타이틀로 돌아가시기 전에 꼭 체크포인트 저장을 하고 진행하시길 바랍니다.",
            "게임종료 버튼입니다. 종료하시기전에 체크포인트 저장을 하고 종료하시길 바랍니다.",
            "ESC 버튼을 눌러 창을 닫을 수 있습니다."
        }));
        pauseUI.BtnActive();
        pauseUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
    }

    // 4. 캐릭터 튜토리얼
    IEnumerator RunCharacterTutorial()
    {
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "TAB키를 눌러 캐릭터의 정보를 볼 수 있습니다.",
            "TAB키를 눌러 보세요."
        }));
        player.Input.UIActions.Enable();
        TextPanel.SetActive(false);
        // 조건: TAB 키 입력 및 캐릭터 정보 창 열림
        yield return new WaitUntil(() => MainMenuUIOn());
        PlayerInputDisable();
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "상태에서 캐릭터의 정보를 확인 할 수 있습니다. 'P'키를 사용해도 확인 할 수 있습니다.",
            "아래의 네모칸은 패시브 아이템 슬롯으로 아이템을 획득하면 자동으로 장착이 됩니다."
        }));

        mainMenuUI.SwapMainMenu(1);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "스킬에서 구입 혹은 획득한 스킬의 설명을 보실 수 있습니다. 장착은 체크포인트 에서만 가능합니다.'K'키를 사용해도 확인 할 수 있습니다.",
        }));
        mainMenuUI.SwapMainMenu(2);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "유물에서는 획득 혹은 구입한 아이템을 확인하실 수 있습니다. 장착은 체크포인트 에서만 가능합니다. 'I'키를 사용해도 확인 할 수 있습니다.",
        }));
        mainMenuUI.SwapMainMenu(3);
        Time.timeScale = 1f;
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "지도에서는 탐험한 곳을 확인할 수 있습니다. 'M'키를 사용해도 확인 할 수 있습니다.",
            "지도를 마우스 드래그로 움직일 수 있습니다.",
            "아래 버튼을 누르면 지도를 원래 위치로 되돌립니다.",
            "ESC 버튼을 눌러 창을 닫을 수 있습니다."
        }));
        mainMenuUI.gameObject.SetActive(false);
        UIManager.Instance.SetOnUI(false);
    }

    bool MainMenuUIOn()
    {
        bool mainMeunOn = false;

        if (mainMenuUI != null)
        {
            if (mainMenuUI.gameObject.activeInHierarchy)
            {
                mainMeunOn = true;
                mainMenuUI.SwapMainMenu(0);
            }
        }
        return mainMeunOn;
    }

    // 5. 전투 튜토리얼
    IEnumerator RunBattleTutorial()
    {
        TextPanel.SetActive(true);
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "정면에 있는 허수아비를 공격해보세요. 공격은 'X'키 입니다."
        }));
        PlayerInputEnable();
        // 조건: 특정 적 또는 오브젝트(허수아비) 공격 성공
        yield return new WaitUntil(() => MonsterDead());

        TextPanel.SetActive(true);
        yield return StartCoroutine(ShowTexts(new string[]
        {
            "좋습니다. 공격은 총 3번까지 사용이 가능하며, 두 번째,세 번째 공격은 더 높은 데미지를 줄 수 있습니다.",
            "'Z'키로 구르기를 사용 할 수 있습니다. 구르기를 사용하는 중에는 몬스터의 공격을 받지 않습니다.",
            "'C'키로 점프를 할 수 있습니다. 높은 곳을 올라갈 때 사용해보세요.",
            "기본적인 조작법은 여기까지 입니다. 진행하시느라 고생하셨습니다.",
            "당신의 모험에 행운이 깃들기를 바랍니다."
        }));
        TutorialCollider.enabled = false;
    }

    bool MonsterDead()
    {
        bool MonsterHit = false;
        if (ScareCrow.HitCount >= 3)
        {
            PlayerInputDisable();
            MonsterHit = true;
        }
        return MonsterHit;
    }


    IEnumerator ShowTexts(string[] texts)
    {
        foreach (var line in texts)
        {
            bool isSkipped = false; 

            Tween tween = null;
            tween = text.DOText(line, 3f).OnUpdate(() =>
            {
                // V키 입력 감지
                if (Input.GetKeyDown(KeyCode.V))
                {
                    tween.Complete(); 
                    isSkipped = true;
                }
            });
            if (isSkipped)
            {
                yield return Delay; 
            }
            else
            {
                yield return waitTextDelay; 
            }
            ClearText();
        }
    }


    private void ClearText()
    {
        text.text = "";
    }

    // 플레이어가 목표 위치에 도달했는지 확인하는 로직
    private bool PlayerHasReachedTargetPosition()
    {
        float distance = Vector3.Distance(player.transform.position, moveTutorialTarget.transform.position);

        return distance <= 5.45f;
    }

}
