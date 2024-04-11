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
    [SerializeReference]
    float               damageDelay;

    float               platformSpeed;
    Vector2             platformVec;

    bool                isFloor;
    bool                isDoubleJump;
    bool                isDamage;
    bool                isPlatform;
    bool                isMove;
    public bool         isAtt;
    bool                isDead;

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
    }

    private void FixedUpdate()
    {
        Flipx();
        MovingPlatform();
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

        if (rigid.velocity.y == 0 || (rigid.velocity.y != 0 && !anim.GetBool("Jump")))  // Velocity.y = 0 ������ �ƴ� ��� || ���������� ���� ���°� �ƴ� ���
        {
            isFloor = true;
            anim.SetBool("Jump", false);
        }
        else
            isFloor = false;


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
        if (collision.CompareTag("Finish"))
            GameManager.instance.PlayerSpawn();

        if (collision.CompareTag("EnemyBullet"))
        {
            StartCoroutine(OnDamage());
            collision.gameObject.SetActive(false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Platform"))
            isPlatform = true;

        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            if (isAtt)
            {
                collision.gameObject.GetComponent<Enemy>().Ondamage();
                rigid.velocity = Vector2.zero;
                rigid.AddForce(transform.up * 10f, ForceMode2D.Impulse);
                isAtt = false;
            }
            else if(!isDamage)
            {
                health--;
                anim.SetTrigger("Hit");

                StartCoroutine(OnDamage());
            }

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

    public void TrapTrigger()
    {
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
        isDamage = true;

        if (health == 0)
            OnDie();

        yield return new WaitForSeconds(damageDelay);
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
