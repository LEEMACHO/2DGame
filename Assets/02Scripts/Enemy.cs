using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [Header("# 0 - 식물   #1 - 치킨     #2 - 버섯     #3 - 토끼     #4 - 슬라임")]
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
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y); //nextMove 에 0:멈춤 -1:왼쪽 1:오른쪽 으로 이동 
            if (rigid.velocity != Vector2.zero)
                anim.SetBool("isRun", true);
            else
                anim.SetBool("isRun", false);
            //Platform check(맵 앞이 낭떨어지면 뒤돌기 위해서 지형을 탐색)


            //자신의 한 칸 앞 지형을 탐색해야하므로 position.x + nextMove(-1,1,0이므로 적절함)
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.6f, rigid.position.y);

            //한칸 앞 부분아래 쪽으로 ray를 쏨
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

            //레이를 쏴서 맞은 오브젝트를 탐지 
            RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));

            //탐지된 오브젝트가 null : 그 앞에 지형이 없음
            if (raycast.collider == null)
            {
                Turn();
            }
        }

    }
    void Think()
    {//몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤  으로 3가지 행동을 판단)

        //Set Next Active
        //Random.Range : 최소<= 난수 <최대 /범위의 랜덤 수를 생성(최대는 제외이므로 주의해야함)
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        //WalkSpeed변수를 nextMove로 초기화 
        //anim.SetInteger("WalkSpeed", nextMove);


        //Flip Sprite
        if (nextMove != 0) //서있을 때 굳이 방향을 바꿀 필요가 없음 
            spriter.flipX = nextMove == 1; //nextmove 가 1이면 방향을 반대로 변경  


        //Recursive (재귀함수는 가장 아래에 쓰는게 기본적) 
        float time = Random.Range(2f, 5f); //생각하는 시간을 랜덤으로 부여 
        //Think(); : 재귀함수 : 딜레이를 쓰지 않으면 CPU과부화 되므로 재귀함수쓸 때는 항상 주의 ->Think()를 직접 호출하는 대신 Invoke()사용
        Invoke("Think", time); //매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행 
    }
    void Turn()
    {

        nextMove = nextMove * (-1); //우리가 직접 방향을 바꾸어 주었으니 Think는 잠시 멈추어야함
        spriter.flipX = nextMove == 1;

        CancelInvoke(); //think를 잠시 멈춘 후 재실행
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
