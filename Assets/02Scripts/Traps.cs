using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public enum Type    { Trap, ChainTrap, Trampoline, Fire, Fan, MovableTrap };
    public Type         trapType;

    public float        windPower;
    public float        trampolinePower;

    bool                isOn;


    BoxCollider2D       coll;
    Animator            anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();


        if (trapType == Type.Fire && !isOn)
        {
            isOn = false;
            coll.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        switch (trapType)
        {
            case Type.Trap:
            case Type.Fire:
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

    public void Fireon()
    {
        Debug.Log("On");
        isOn = true;
        StartCoroutine(FireOn());
    }

    public void Fireoff()
    {
        Debug.Log("Off");
        isOn = false;
        StopCoroutine(FireOn());
    }
    IEnumerator FireOn()
    {
        while(isOn)
        {
            yield return new WaitForSeconds(1f);

            coll.enabled = true;
            anim.SetBool("isOn", true);

            yield return new WaitForSeconds(1f);
            coll.enabled = false;
            anim.SetBool("isOn", false);
        }
        yield return new WaitForSeconds(1f);
    }
}
