using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCtr : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.instance.PlayerSpawn();
        }
    }
}
