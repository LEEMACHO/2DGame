using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeReference]
    float           delay;

    bool            isOn;

    SpriteRenderer  sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player"))
            return;
        isOn = true;
        StartCoroutine(Broken());
        Debug.Log(isOn);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player"))
            return;

        isOn = false;
        Debug.Log(isOn);
    }

    IEnumerator Broken()
    {

        sprite.flipX = true;
        yield return new WaitForSeconds(0.5f);
        sprite.flipX = false;
        yield return new WaitForSeconds(0.5f);
        sprite.flipX = true;
        yield return new WaitForSeconds(0.5f);
        sprite.flipX = false;
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }


}
