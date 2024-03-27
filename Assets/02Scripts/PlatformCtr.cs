using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCtr : MonoBehaviour
{
    public Vector2 movementVector = new Vector2(0f, 0f); // ������ �̵� ����� �Ÿ�
    public float speed = 2f; // ������ �̵� �ӵ�

    private Vector3 startPosition; // ������ ���� ��ġ
    private Rigidbody2D rb; // ������ Rigidbody2D ������Ʈ

    private void Awake()
    {
        // ������ ���� ��ġ�� ����
        startPosition = transform.position;
        // ������ Rigidbody2D ������Ʈ�� ��������
        rb = GetComponent<Rigidbody2D>();
        // ������ �̵� �������� ������ �ӵ��� �̵���Ű��
        rb.velocity = movementVector.normalized * speed;
    }

    void FixedUpdate()
    {
        // ������ ���� ��ġ�� ���� �Ÿ���ŭ �̵��ϸ�
        if (Vector3.Distance(transform.position, startPosition) >= movementVector.magnitude)
        {
            // �̵� ������ �ݴ�� �����Ͽ� ������ �ǵ��ư��� ����ϴ�.
            movementVector *= -1;
            // ������ �̵� �������� ������ �ӵ��� �̵���Ű��
            rb.velocity = movementVector.normalized * speed;
        }
    }

    // ���ǰ� �÷��̾ �浹�� ���
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾ ������ �ڽ����� �����Ͽ� �Բ� �̵��ϰ� ����ϴ�.
            collision.transform.SetParent(transform);
        }
    }

    // ���ǰ� �÷��̾��� �浹�� ���� ���
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾��� �θ� �ʱ�ȭ�Ͽ� ���ǰ��� �θ�-�ڽ� ���踦 �����մϴ�.
            collision.transform.SetParent(null);
        }
    }

}
