using System.Collections;
using UnityEngine;

public class ThornTrap : Trap
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyDamage();
        }
    }

}
