using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class FastTravelUI : UIBase
{
    public Button buttonPrefab;
    public GameObject buttonGroup;

    private Dictionary<string, Button> buttons = new Dictionary<string, Button>();

    private void Awake()
    {
        InitializeCheckpoint();        
    }

    private void OnEnable()
    {
        GameManager.Instance.mapCamera.orthographicSize = 60f;        
        buttonGroup.transform.GetChild(0).GetComponent<Button>().Select();
    }

    private void Start()
    {
    }

    private void Update()
    {        
        UpdateCheckpoint();
    }

    private void OnDisable()
    {
        GameManager.Instance.mapCamera.orthographicSize = 25f;

    }

    public void OnClickCheckpointButton(string checkpointName)
    {        
        CheckpointManager.Instance.Teleport(checkpointName);        
        UIManager.Instance.ToggleUI(ref UIManager.Instance.fastTravelUI, 0f, 1f, false, true);
    }

    private void InitializeCheckpoint()
    {
        foreach (var checkpoint in CheckpointManager.Instance.checkpoints.Values)
        {            
            Button button = Instantiate(buttonPrefab, buttonGroup.transform);
            button.transform.SetAsFirstSibling();            
            button.GetComponentInChildren<TextMeshProUGUI>().text = checkpoint.checkPointName;
            button.onClick.AddListener(() => OnClickCheckpointButton(checkpoint.checkPointName));
            button.gameObject.AddComponent<FastTravelBtn>().Init(this, checkpoint.checkpointIcon.transform.position);
            
            button.gameObject.SetActive(false);
            buttons.Add(checkpoint.checkPointName, button);
        }
    }

    private void UpdateCheckpoint()
    {        
        foreach (var checkpoint in CheckpointManager.Instance.checkpoints.Values)
        {            
            if (buttons.TryGetValue(checkpoint.checkPointName, out Button button))
            {
                button.gameObject.SetActive(checkpoint.isDiscovered);                
            }
        }
    }

    public void OnCheckpointButton(Vector3 checkpointPosition)
    {
        MoveMapToCheckpoint(checkpointPosition);
    }

    private void MoveMapToCheckpoint(Vector3 checkpointPosition)
    {
        GameManager.Instance.mapCamera.transform.position = checkpointPosition + new Vector3(0, 0, -30f);           
    }
}
