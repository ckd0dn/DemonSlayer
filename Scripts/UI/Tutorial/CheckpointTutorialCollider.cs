using System.Collections;
using UnityEngine;

public class CheckpointTutorialCollider : MonoBehaviour
{
    public GameObject CheckpointTutorialMessage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(TogglerMessage());
    }

    IEnumerator TogglerMessage()
    {
        CheckpointTutorialMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        CheckpointTutorialMessage.SetActive(false);
    }
}
