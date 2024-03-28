using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [Header("# 0 - �Ĺ�   #1 - ġŲ     #2 - ����     #3 - �䳢     #4 - ������")]
    public int                          type;
    [Header("# Enemy State")]
    public float                        health;
    public float                        maxHealth;

    int                                 nextMove;
    float delay;

    Rigidbody2D                         rigid;
    Animator                            anim;
    SpriteRenderer                      spriter;
    public Transform bulletPos;
    public GameObject bullet;
    public float bulletSpeed;
    RuntimeAnimatorController[] animCons;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Invoke("Think", 5);
    }
    void FixedUpdate()
    {
        Action();
    }

    void Action()
    {

        if(type == 0)
        {
            delay += Time.fixedDeltaTime;

            if (delay > 1f)
            {
                GameObject instantBullet = Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
                Rigidbody2D rigidBullet = instantBullet.GetComponent<Rigidbody2D>();
                rigidBullet.velocity = bulletPos.right * -bulletSpeed;
                delay = 0f;
            }
        }
        else
        {
            //Move
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y); //nextMove �� 0:���� -1:���� 1:������ ���� �̵� 
            if (rigid.velocity != Vector2.zero)
                anim.SetBool("isRun", true);
            else
                anim.SetBool("isRun", false);
            //Platform check(�� ���� ���������� �ڵ��� ���ؼ� ������ Ž��)


            //�ڽ��� �� ĭ �� ������ Ž���ؾ��ϹǷ� position.x + nextMove(-1,1,0�̹Ƿ� ������)
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.6f, rigid.position.y);

            //��ĭ �� �κоƷ� ������ ray�� ��
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

            //���̸� ���� ���� ������Ʈ�� Ž�� 
            RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));

            //Ž���� ������Ʈ�� null : �� �տ� ������ ����
            if (raycast.collider == null)
            {
                Turn();
            }
        }

    }
    void Think()
    {//���Ͱ� ������ �����ؼ� �Ǵ� (-1:�����̵� ,1:������ �̵� ,0:����  ���� 3���� �ൿ�� �Ǵ�)

        //Set Next Active
        //Random.Range : �ּ�<= ���� <�ִ� /������ ���� ���� ����(�ִ�� �����̹Ƿ� �����ؾ���)
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        //WalkSpeed������ nextMove�� �ʱ�ȭ 
        //anim.SetInteger("WalkSpeed", nextMove);


        //Flip Sprite
        if (nextMove != 0) //������ �� ���� ������ �ٲ� �ʿ䰡 ���� 
            spriter.flipX = nextMove == 1; //nextmove �� 1�̸� ������ �ݴ�� ����  


        //Recursive (����Լ��� ���� �Ʒ��� ���°� �⺻��) 
        float time = Random.Range(2f, 5f); //�����ϴ� �ð��� �������� �ο� 
        //Think(); : ����Լ� : �����̸� ���� ������ CPU����ȭ �ǹǷ� ����Լ��� ���� �׻� ���� ->Think()�� ���� ȣ���ϴ� ��� Invoke()���
        Invoke("Think", time); //�Ű������� ���� �Լ��� time���� �����̸� �ο��Ͽ� ����� 
    }
    void Turn()
    {

        nextMove = nextMove * (-1); //�츮�� ���� ������ �ٲپ� �־����� Think�� ��� ���߾����
        spriter.flipX = nextMove == 1;

        CancelInvoke(); //think�� ��� ���� �� �����
        Invoke("Think", 2);//  

    }
    public void OnDamage()
    {
        health--;

        if (health < 0)
            OnDie();

        anim.SetTrigger("Hit");
        
    }
    void OnDie()
    {
        gameObject.SetActive(false);
    }
}
