using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StEnemy : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            GameManager.instance.player.isAtt = true;
    }

}
