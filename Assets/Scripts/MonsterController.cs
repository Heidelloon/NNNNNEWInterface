using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform player;
    public Transform hammer; // �ظ��� ��ġ�� �������� ���� ����
    public float speed = 5f;
    public float range = 10f;
    public float chaseCooldown = 5f; // �i�ư��� �簳������ ��� �ð�

    private bool canChase = true; // �i�ư��� ���� ���θ� ��Ÿ���� ����
    private float chaseTimer = 0f; // �i�ư��� ��� Ÿ�̸�

    void Update()
    {
        if (canChase)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= range)
            {
                transform.LookAt(player);
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
        else
        {
            // �i�ư��� ���� ���ΰ� false�� ���, ��ٿ� Ÿ�̸Ӹ� ������Ʈ
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseCooldown)
            {
                // ��ٿ��� ������ �ٽ� �i�ư��� �����ϵ��� ����
                canChase = true;
                chaseTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
    }

    // ���Ϳ� �ظ� ������ �浹�� �����ϴ� �޼���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hammer"))
        {
            // �浹�� ������Ʈ�� �ظ��� �� �i�ư��� �ʵ��� ����
            canChase = false;
        }
    }
}
