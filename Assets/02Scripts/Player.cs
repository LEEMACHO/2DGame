using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("# Player State")]
    public float        health;
    public float        speed;
    public float        jumpPower;
    public Vector2      inputVec;

    float platformSpeed;
    Vector2 platformVec;

    bool                isFloor;
    bool                isDoubleJump;
    bool                isDamage;
    bool                isPlatform;
    bool                isMove;
    bool                isAtt;
    public bool         isDead;

    Rigidbody2D         rigid;
    SpriteRenderer      spriter;
    Animator            anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isDead = false;
    }

    private void Update()
    {
        Move();
        Jump();
        Flipx();
        MovingPlatform();
    }

    private void FixedUpdate()
    {
        EnemyCheck();
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
        if (isDead)
            return;
        
        float moveInput = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
        anim.SetBool("isRun", true);
        inputVec = new Vector2(moveInput * speed, rigid.velocity.y).normalized;
        isMove = false;

        if (inputVec == Vector2.zero)
        {
            isMove = true;
            anim.SetBool("isRun", false);
        }
        if(isPlatform && (inputVec.x == 0))
            anim.SetBool("isRun", false);
    }
    void Jump()
    {
        if (isDead)
            return;

        if (rigid.velocity.y == 0 || (rigid.velocity.y != 0 && !anim.GetBool("Jump")))  // Velocity.y = 0 공중이 아니 경우 || 공중이지만 점프 상태가 아닌 경우
        {
            isFloor = true;
            anim.SetBool("Jump", false);
        }
        else
            isFloor = false;

        //if (isFloor)
        //    isDoubleJump = true;


        if (Input.GetButtonDown("Jump") && !anim.GetBool("Jump") && isFloor)
        {
            isPlatform = false;
            Debug.Log("Jump");
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

        if(isPlatform)
            anim.SetBool("Jump", false);
    }
    void MovingPlatform()
    {
        if (isPlatform && isMove)
            rigid.velocity = platformVec.normalized * platformSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutLine"))
            GameManager.instance.Retry();
        else if (collision.CompareTag("Finish"))
            GameManager.instance.PlayerSpawn();     

        if(collision.CompareTag("EnemyBullet"))
            StartCoroutine(OnDamage());

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Platform"))
            isPlatform = true;

        if(collision.collider.gameObject.CompareTag("Enemy"))
        {
            if (isAtt)
                collision.gameObject.GetComponent<Enemy>().OnDamage();
            else
                StartCoroutine(OnDamage());
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Platform"))
        {
            platformSpeed = collision.gameObject.GetComponent<PlatformCtr>().speed;
            platformVec = collision.gameObject.GetComponent<PlatformCtr>().movementVector;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Platform"))
            isPlatform = false;
    }
    void EnemyCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down, new Color(0, 1, 0));
        isAtt = Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Enemy"));
    }
    public void TrapTrigger()
    {
        if (isDamage)
            return;
        StartCoroutine(OnDamage());
    }
    public void WIndJump(float power)
    {
        rigid.AddForce(transform.up * power, ForceMode2D.Force);
    }
    public void TrampolineJump(float power)
    {
        anim.SetBool("Jump", false);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(transform.up * power, ForceMode2D.Impulse);
    }
    IEnumerator OnDamage()
    {
        health--;

        if (health<1)
        {
            OnDie();
        }

        anim.SetTrigger("Hit");
        isDamage = true;

        yield return new WaitForSeconds(0.5f);

        isDamage = false;
    }
    void OnDie()
    {
        anim.StopPlayback();
        gameObject.SetActive(false);
        isDead = true;
        GameManager.instance.GameOver();
    }
}
