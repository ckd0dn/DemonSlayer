using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private FadeEffect fadedEffect;
    private Vector3 respawnPosition;
    public bool isRespawn;
    private void Start()
    {
        respawnPosition = transform.position;

        if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY"))
        {
            float checkpointX = PlayerPrefs.GetFloat("CheckpointX");
            float checkpointY = PlayerPrefs.GetFloat("CheckpointY");
            respawnPosition = new Vector3(checkpointX, checkpointY, transform.position.z);
            transform.position = respawnPosition;
        }
    }
        public void SetCheckpoint(Vector3 newCheckpoint)
    {
        respawnPosition = newCheckpoint;
        PlayerPrefs.SetFloat("CheckpointX", newCheckpoint.x);
        PlayerPrefs.SetFloat("CheckpointY", newCheckpoint.y);
        PlayerPrefs.Save();
    }


}
