using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public enum Type { StandEnemy, MoveEnemy, ReactionEneny, AttEnemy, SlimeEnemy}
    public Type enemyType;

    [Header("# Enemy State")]
    public float                        health;
    public float                        maxHealth;
    public float                        speed = 1f;
    [Header("# Enemy Info")]
    [SerializeReference]
    int                                 nextMove = 1;
    float                               delay;
    [SerializeReference]
    float                               AttDelay;

    public bool                         isDamage;
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
            case Type.SlimeEnemy:
            case Type.MoveEnemy:
                //Move
                rigid.velocity = new Vector2(speed * nextMove, rigid.velocity.y);
                anim.SetBool(enemyType == Type.ReactionEneny ? "isWalk" : "isRun",true);
                spriter.flipX = dir = nextMove == 1 ? true : false;

                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.6f, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
                RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Floor"));

                if (raycast.collider == null)
                    nextMove *= -1;

                break;

            case Type.StandEnemy:
                delay += Time.deltaTime;

                if (delay > AttDelay)
                {
                    GameObject instantBullet = Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
                    Rigidbody2D rigidBullet = instantBullet.GetComponent<Rigidbody2D>();
                    nextMove = spriter.flipX == true ? 1 : -1;
                    rigidBullet.velocity = bulletPos.right * bulletSpeed * nextMove;
                    delay = 0f;
                }
                break;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PlayerBullet"))
            return;
        
    }

    public void Ondamage()
    {
        health--;
        anim.SetTrigger("Hit");
        isDamage = false;

        if (health == 0)
            OnDie();

        if (enemyType == Type.SlimeEnemy)
        {
            GameObject[] instantObj = new GameObject[2];
            for (int index = 0; index < 2; index++)
            {
                instantObj[index] = Instantiate(gameObject, new Vector3(transform.position.x + Random.Range(-1, 2), transform.position.y, transform.position.z), transform.rotation);
                Debug.Log(instantObj[index].gameObject.transform.position);
                Enemy enemyObj = instantObj[index].GetComponent<Enemy>();
                enemyObj.maxHealth = 1;
                enemyObj.health = 1;
                enemyObj.speed = 2;
                enemyObj.nextMove = Random.Range(-1, 1);
                if (enemyObj.nextMove == 0)
                    enemyObj.nextMove = 1;
                enemyObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            gameObject.SetActive(false);
        }

        if (enemyType == Type.ReactionEneny)
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isWalk", false);
            speed *= 2;
        }

    }

    /*
    IEnumerator OnDamage()
    {
        health--;
        anim.SetTrigger("Hit");
        isDamage = false;

        if (health == 0)
            OnDie();

        if (enemyType == Type.SlimeEnemy)
        {
            GameObject[] instantObj = new GameObject[2];
            for (int index = 0; index < 2; index++)
            {
                instantObj[index] = Instantiate(gameObject, new Vector3(transform.position.x + Random.Range(-1, 2), transform.position.y, transform.position.z), transform.rotation);
                Debug.Log(instantObj[index].gameObject.transform.position);
                Enemy enemyObj = instantObj[index].GetComponent<Enemy>();
                enemyObj.maxHealth = 1;
                enemyObj.health = 1;
                enemyObj.speed = 2;
                enemyObj.nextMove = Random.Range(-1, 1);
                if (enemyObj.nextMove == 0)
                    enemyObj.nextMove = 1;
                enemyObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            gameObject.SetActive(false);
        }

        if (enemyType == Type.ReactionEneny)
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isWalk", false);
            speed *= 2;
        }

        yield return new WaitForSeconds(damageDelay);


    }
    */

    void OnDie()
    {
        gameObject.SetActive(false);
    }
}
