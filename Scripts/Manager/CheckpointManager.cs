using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : Singleton<CheckpointManager>
{
    private Player player;
    public Dictionary<string, CheckPoint> checkpoints = new Dictionary<string, CheckPoint>();

    public void Teleport(string checkpointName)
    {
        if (checkpoints.TryGetValue(checkpointName, out CheckPoint checkpoint))
        {
            if (checkpoint.isDiscovered)
            {                
                checkpoint.transform.parent.gameObject.SetActive(true);
                player = GameManager.Instance.Player;
                player.transform.position = checkpoint.transform.position;
            }
        }
    }    

    public void Init()
    {
        foreach (var checkpoint in FindObjectsOfType<CheckPoint>())
        {
            if (!checkpoints.ContainsKey(checkpoint.checkPointName))
            {
                checkpoints.Add(checkpoint.checkPointName, checkpoint);
            }
        }
    }
}
