using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtr : MonoBehaviour
{
    public Vector2 movementVector = new Vector2(0f, 0f); // 발판의 이동 방향과 거리
    public float speed = 2f; // 발판의 이동 속도

    private Vector3 startPosition; // 발판의 시작 위치
    private Rigidbody2D rb; // 발판의 Rigidbody2D 컴포넌트

    private void Awake()
    {
        // 발판의 시작 위치를 설정
        startPosition = transform.position;
        // 발판의 Rigidbody2D 컴포넌트를 가져오기
        rb = GetComponent<Rigidbody2D>();
        // 발판을 이동 방향으로 설정된 속도로 이동시키기
        rb.velocity = movementVector.normalized * speed;
    }

    void FixedUpdate()
    {
        // 발판이 시작 위치와 일정 거리만큼 이동하면
        if (Vector3.Distance(transform.position, startPosition) >= movementVector.magnitude)
        {
            // 이동 방향을 반대로 변경하여 발판을 되돌아가게 만듭니다.
            movementVector *= -1;
            // 발판을 이동 방향으로 설정된 속도로 이동시키기
            rb.velocity = movementVector.normalized * speed;
        }
    }

    // 발판과 플레이어가 충돌한 경우
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어를 발판의 자식으로 설정하여 함께 이동하게 만듭니다.
            collision.transform.SetParent(transform);
        }
    }

    // 발판과 플레이어의 충돌이 끝난 경우
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어의 부모를 초기화하여 발판과의 부모-자식 관계를 해제합니다.
            collision.transform.SetParent(null);
        }
    }

}
