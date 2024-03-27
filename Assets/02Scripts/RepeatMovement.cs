using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatMovement : MonoBehaviour
{
    public Transform                startPos;
    public Transform                endPos;
    public float                    speed;
    public float                    delay;

    public bool                            movingToEnd = true;


    private void Awake()
    {
        StartCoroutine(MoveOn());
    }

    IEnumerator MoveOn()
    {

        while(true)
        {
            // 시작 지점에서 종료 지점으로 이동하는 경우
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(delay);
                // 종료 지점에 도달한 경우
                if (Vector3.Distance(transform.position, endPos.position) < 0.001f)
                {
                    movingToEnd = false; // 이동 방향 변경
                    yield return new WaitForSeconds(1f);
                }
            }
            // 종료 지점에서 시작 지점으로 이동하는 경우
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(delay);
                // 시작 지점에 도달한 경우
                if (Vector3.Distance(transform.position, startPos.position) < 0.001f)
                {
                    movingToEnd = true; // 이동 방향 변경
                    yield return new WaitForSeconds(1f);
                }
            }

        }
    }


}
