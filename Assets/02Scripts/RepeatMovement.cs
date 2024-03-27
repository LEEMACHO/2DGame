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
            // ���� �������� ���� �������� �̵��ϴ� ���
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(delay);
                // ���� ������ ������ ���
                if (Vector3.Distance(transform.position, endPos.position) < 0.001f)
                {
                    movingToEnd = false; // �̵� ���� ����
                    yield return new WaitForSeconds(1f);
                }
            }
            // ���� �������� ���� �������� �̵��ϴ� ���
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos.position, speed * Time.deltaTime);
                yield return new WaitForSeconds(delay);
                // ���� ������ ������ ���
                if (Vector3.Distance(transform.position, startPos.position) < 0.001f)
                {
                    movingToEnd = true; // �̵� ���� ����
                    yield return new WaitForSeconds(1f);
                }
            }

        }
    }


}
