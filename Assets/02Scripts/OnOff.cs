using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    BoxCollider2D[] onObjs;
    SpriteRenderer[] sprites;

    [SerializeReference]
    bool isOn;

    private void Awake()
    {
        onObjs = GetComponentsInChildren<BoxCollider2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        Onoff();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        isOn = isOn == true ? false : true;
        Onoff();
    }

    void Onoff()
    {
        if(!isOn)
        for (int index = 1; index < sprites.Length; index++)
        {
            onObjs[index].enabled = false;
            sprites[index].color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            for (int index = 1; index < sprites.Length; index++)
            {
                onObjs[index].enabled = true;
                sprites[index].color = new Color(1, 1, 1, 1);
            }
        }
    }
}
