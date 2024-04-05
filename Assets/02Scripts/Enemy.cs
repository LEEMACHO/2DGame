using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public enum Type { StandEnemy, MoveEnemy, ReactionEneny, AttEnemy}
    public Type enemyType;

    [Header("# Enemy State")]
    public float                        health;
    public float                        maxHealth;
    public float                        speed = 1f;
    [Header("# Enemy Info")]
    [SerializeReference]
    int                                 nextMove = 1;
    float                               delay;
    bool                                dir;


    Rigidbody2D                         rigid;
    Animator                            anim;
    SpriteRenderer                      spriter;
    public Transform                    bulletPos;
    public GameObject                   bullet;
    public float                        bulletSpeed;
    //RuntimeAnimatorController[] animCons;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //if (enemyType == Type.MoveEnemy || enemyType == Type.ReactionEneny)
        //    StartCoroutine(OnThink());
    }

    private void FixedUpdate()
    {
        Action();
    }

    void Action()
    {

        switch (enemyType)
        {
            case Type.ReactionEneny:
            case Type.MoveEnemy:
                //Move
                rigid.velocity = Vector2.right * speed * nextMove;
                anim.SetBool(enemyType == Type.MoveEnemy ? "isRun" : "isWalk",true);
                spriter.flipX = dir = nextMove == 1 ? true : false;

                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 1f, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
                RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));

                if (raycast.collider == null)
                    nextMove = nextMove * -1;

                break;

            case Type.StandEnemy:
                delay += Time.deltaTime;

                if (delay > 1f)
                {
                    GameObject instantBullet = Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
                    Rigidbody2D rigidBullet = instantBullet.GetComponent<Rigidbody2D>();
                    rigidBullet.velocity = bulletPos.right * -bulletSpeed;
                    delay = 0f;
                }
                break;
        }


    }
    //void Think()
    //{//���Ͱ� ������ �����ؼ� �Ǵ� (-1:�����̵� ,1:������ �̵� ,0:����  ���� 3���� �ൿ�� �Ǵ�)

    //    //Set Next Active
    //    //Random.Range : �ּ�<= ���� <�ִ� /������ ���� ���� ����(�ִ�� �����̹Ƿ� �����ؾ���)
    //    nextMove = Random.Range(-1, 2);

    //    //Sprite Animation
    //    //WalkSpeed������ nextMove�� �ʱ�ȭ 
    //    //anim.SetInteger("WalkSpeed", nextMove);


    //    //Flip Sprite
    //    if (nextMove != 0) //������ �� ���� ������ �ٲ� �ʿ䰡 ���� 
    //        spriter.flipX = nextMove == 1; //nextmove �� 1�̸� ������ �ݴ�� ����  


    //    //Recursive (����Լ��� ���� �Ʒ��� ���°� �⺻��) 
    //    float time = Random.Range(1f, thinkDelay); //�����ϴ� �ð��� �������� �ο� 
    //    //Think(); : ����Լ� : �����̸� ���� ������ CPU����ȭ �ǹǷ� ����Լ��� ���� �׻� ���� ->Think()�� ���� ȣ���ϴ� ��� Invoke()���
    //    Invoke("Think", time); //�Ű������� ���� �Լ��� time���� �����̸� �ο��Ͽ� ����� 
    //}
    //IEnumerator OnThink()
    //{
    //    //isThink = false;

    //    yield return new WaitForSeconds(thinkDelay);
    //    nextMove = Random.Range(-1, 2);

    //    if (nextMove != 0)
    //        spriter.flipX = nextMove == 1;

    //    float time = Random.Range(1f, thinkDelay);

    //    yield return new WaitForSeconds(time);

    //    //isThink = true;
    //}
    public void OnDamage()
    {
        if (enemyType == Type.ReactionEneny)
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isWalk", false);
            speed++;
        }

        health--;
        speed++;

        if (health < 1)
            OnDie();

        anim.SetTrigger("Hit");
        
    }
    void OnDie()
    {
        gameObject.SetActive(false);
    }
}
