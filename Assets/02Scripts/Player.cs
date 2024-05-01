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

    [Header("# Player Info")]
    [SerializeReference]
    float               damageDelay;
    float               dir;                    // -1 왼쪽, +1 오른쪽
    float               platformSpeed;
    [SerializeReference]
    float               wallDistance;
    [SerializeReference]
    float               wallSpeed;
    Vector2             platformVec;
    Vector2             jumpVec;
    [SerializeReference]
    float wallJumpPower;


    [SerializeReference]
    bool                isFloor;
    bool                isDoubleJump;
    bool                isDamage;
    bool                isPlatform;
    bool                isMove;
    public bool         isAtt;
    bool                isDead;
    [SerializeReference]
    bool                isWall;

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

    private void Start()
    {
        jumpVec = new Vector2(1, 1);
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
        WallJump();
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
        if (isDead || isWall)
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

        if (rigid.velocity.y == 0 || (rigid.velocity.y != 0 && !anim.GetBool("isJump")))  // Velocity.y = 0 ������ �ƴ� ��� || ���������� ���� ���°� �ƴ� ���
        {
            isFloor = true;
            anim.SetBool("isJump", false);
        }
        else
            isFloor = false;

        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump") && isFloor)
        {
            isPlatform = false;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(isWall == true ? new Vector2(jumpVec.x * wallJumpPower * (-dir), jumpVec.y * wallJumpPower) : Vector2.up * jumpPower, ForceMode2D.Impulse); 
            anim.SetBool("isJump",true);
            isDoubleJump = true;
        }
        else if (Input.GetButtonDown("Jump")&& anim.GetBool("isJump") && isDoubleJump)
        {
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("DoubleJump");

            isDoubleJump = false;
        }

        if (isWall)
            anim.SetBool("isJump", false);

        if(isPlatform)
            anim.SetBool("isJump", false);
    }
    void MovingPlatform()
    {
        if (isPlatform && isMove)
            rigid.velocity = platformVec.normalized * platformSpeed;
    }
    void WallJump()
    {
        dir = spriter.flipX == true ? -1 : 1;
        isWall = Physics2D.Raycast(transform.position, Vector2.right * dir, wallDistance,LayerMask.GetMask("Wall"));
        Debug.DrawRay(transform.position, Vector3.right * dir, Color.green);
        anim.SetBool("isWall", isWall);

        if (isWall)
        {
            anim.SetBool("isRun", false);
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * wallSpeed);
        }

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

        if (Physics2D.Raycast(transform.position, Vector3.down, 1, LayerMask.GetMask("Floor")))
            anim.SetBool("isJump", false);

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
        anim.SetBool("isJump", false);
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
