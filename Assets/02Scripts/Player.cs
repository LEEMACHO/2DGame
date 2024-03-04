using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public Vector2 inputVec;

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
    }

    private void FixedUpdate()
    {
        //isJump();
    }


    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
        anim.SetBool("isRun", true);

        inputVec = new Vector2(moveInput * speed, rigid.velocity.y).normalized;
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }

        if (inputVec == Vector2.zero)
            anim.SetBool("isRun", false);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            anim.SetTrigger("Jump");
        }
    }

    void isJump()
    {
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Floor"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.3f)
                {
                    anim.SetBool("isJump", false);
                }
            }
        }
    }


}
