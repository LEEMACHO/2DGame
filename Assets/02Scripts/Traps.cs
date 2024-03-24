using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public enum Type    { Trap, ChainTrap, Trampoline, RepeatTrap, Fan, MovableTrap };
    public Type         trapType;

    public float        windPower;
    public float        trampolinePower;

    Animator            anim;

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
            case Type.Trap:
            case Type.RepeatTrap:
                GameManager.instance.player.TrapTrigger();
                break;
            case Type.MovableTrap:
                GameManager.instance.player.TrapTrigger();
                anim.SetTrigger("Attack");
                break;
            case Type.Trampoline:
                GameManager.instance.player.TrampolineJump(trampolinePower);
                anim.SetTrigger("Jump");
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        switch (trapType)
        {
            case Type.Fan:
                GameManager.instance.player.WIndJump(windPower);  
                break;
        }
    }

    public void StopObj()
    {
        gameObject.SetActive(false);
    }

}
