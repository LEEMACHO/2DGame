using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutLine") || collision.CompareTag("Floor"))
            gameObject.SetActive(false);
    }
}
