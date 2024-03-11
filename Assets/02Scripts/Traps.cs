using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public enum Type { Trampoline, Trap};
    public Type trapType;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        switch (trapType)
        {
            case Type.Trampoline:
                anim.SetTrigger("Jump");
                break;

            case Type.Trap:

                break;
        }
    }


}
