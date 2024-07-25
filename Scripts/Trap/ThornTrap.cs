using System.Collections;
using UnityEngine;

public class ThornTrap : Trap
{
    private Coroutine coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coroutine = StartCoroutine(ApplyDamageCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator ApplyDamageCoroutine()
    {
        while (true)
        {
            ApplyDamage();
            yield return new WaitForSeconds(0.5f);
        }
    }

}
