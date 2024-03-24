using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public enum Type { Updown,Rot };
    public Type trapType;

    [Header("# Updown Obj")]
    public GameObject obj;
    public BoxCollider2D coll;
    public Transform fallPos;

    public float                    fallSpeed; // 내려찍는 속도
    public float                    riseSpeed; // 올라오는 속도
    public float                    delayTime; // 내려찍는 시간

    [Header("# Rot Obj")]
    public float                    rotSpeed;


    Animator anim;
    private void Awake()
    {
        switch (trapType)
        {
            case Type.Updown:
                anim = obj.GetComponent<Animator>();
                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        switch (trapType)
        {
            case Type.Updown:
                StartCoroutine(Fall());
                break;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;
        switch (trapType)
        {
            case Type.Rot:
                transform.Rotate(Vector3.back * rotSpeed * Time.deltaTime);
                break;
        }
    }



    IEnumerator Fall()
    {
        coll.enabled = true;
        anim.SetTrigger("Attack");

        while (obj.transform.position.y > fallPos.transform.position.y)
        {
            obj.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delayTime);

        coll.enabled = false;

        while (obj.transform.position.y < transform.position.y)
        {
            obj.transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
