using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public GameObject TutorialMessage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(TogglerMessage());
    }

    IEnumerator TogglerMessage()
    {
        if(TutorialMessage != null)
        {
            TutorialMessage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            TutorialMessage.SetActive(false);
        }
    }
}
