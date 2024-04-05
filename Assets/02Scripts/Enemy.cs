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
    //{//몬스터가 스스로 생각해서 판단 (-1:왼쪽이동 ,1:오른쪽 이동 ,0:멈춤  으로 3가지 행동을 판단)

    //    //Set Next Active
    //    //Random.Range : 최소<= 난수 <최대 /범위의 랜덤 수를 생성(최대는 제외이므로 주의해야함)
    //    nextMove = Random.Range(-1, 2);

    //    //Sprite Animation
    //    //WalkSpeed변수를 nextMove로 초기화 
    //    //anim.SetInteger("WalkSpeed", nextMove);


    //    //Flip Sprite
    //    if (nextMove != 0) //서있을 때 굳이 방향을 바꿀 필요가 없음 
    //        spriter.flipX = nextMove == 1; //nextmove 가 1이면 방향을 반대로 변경  


    //    //Recursive (재귀함수는 가장 아래에 쓰는게 기본적) 
    //    float time = Random.Range(1f, thinkDelay); //생각하는 시간을 랜덤으로 부여 
    //    //Think(); : 재귀함수 : 딜레이를 쓰지 않으면 CPU과부화 되므로 재귀함수쓸 때는 항상 주의 ->Think()를 직접 호출하는 대신 Invoke()사용
    //    Invoke("Think", time); //매개변수로 받은 함수를 time초의 딜레이를 부여하여 재실행 
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
