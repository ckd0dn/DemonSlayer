using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : Singleton<CheckpointManager>
{
    private Player player;
    public Dictionary<string, CheckPoint> checkpoints = new Dictionary<string, CheckPoint>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {        
        player = GameManager.Instance.Player;

        foreach (var checkpoint in FindObjectsOfType<CheckPoint>())
        {
            if (!checkpoints.ContainsKey(checkpoint.checkPointName))
            {
                checkpoints.Add(checkpoint.checkPointName, checkpoint);                
            }
        }
    }

    public void Teleport(string checkpointName)
    {
        if (checkpoints.TryGetValue(checkpointName, out CheckPoint checkpoint))
        {
            if (checkpoint.isDiscovered)
            {
                checkpoint.checkpointRoom.gameObject.SetActive(true);
                player.transform.position = checkpoint.transform.position;
            }
        }
    }    
}
