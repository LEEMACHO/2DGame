using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float health;

    public float speed;
    public float jumpPower;
    public Vector2 inputVec;
    public float jumpdelay;


    bool isFloor;
    bool isDoubleJump;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        Flipx();
    }

    private void FixedUpdate()
    {
        //Isfloor();
    }


    void Flipx()
    {
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
        anim.SetBool("isRun", true);

        inputVec = new Vector2(moveInput * speed, rigid.velocity.y).normalized;

        if (inputVec == Vector2.zero)
            anim.SetBool("isRun", false);
    }

    void Jump()
    {
        if (rigid.velocity.y == 0 || (rigid.velocity.y != 0 && !anim.GetBool("Jump")))
        {
            isFloor = true;
            anim.SetBool("Jump", false);
        }
        else
            isFloor = false;

        if (isFloor)
            isDoubleJump = true;


        if (Input.GetButtonDown("Jump") && !anim.GetBool("Jump") && isFloor )
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("Jump",true);

            isDoubleJump = true;
        }
        else if (Input.GetButtonDown("Jump")&& anim.GetBool("Jump") && isDoubleJump)
        {
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("DoubleJump");

            isDoubleJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutLine"))
        {
            StageManager.instance.Retry();
            health--;
            if(health == 0)
            {
                Debug.Log("���� ����");
            }
        }
    }

}
