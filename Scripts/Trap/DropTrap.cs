using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrap : Trap
{
    public Transform RespawnPosition;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        ApplyDamage();
        ReSpawnPlayer();
    }

    private void ReSpawnPlayer()
    {
        GameManager.Instance.Player.transform.position = RespawnPosition.position;
    }
}
