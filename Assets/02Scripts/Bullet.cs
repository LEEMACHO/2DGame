using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    float stopTime;
    [SerializeReference]
    float stoptimeSet;

    private void Update()
    {
        stopTime += Time.deltaTime;

        if (stopTime > stoptimeSet)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutLine") || collision.CompareTag("Floor"))
            gameObject.SetActive(false);
    }
}
