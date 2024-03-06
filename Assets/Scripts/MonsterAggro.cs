using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAggro : MonoBehaviour
{
    public float aggroRadius = 10f; // ��׷� �ݰ�
    public float maxAggroDistance = 20f; // ��׷� ���� �ִ� �Ÿ�
    public AudioClip aggroSound; // ��׷� ����

    private bool isAggroed = false; // ��׷� ���¸� ��Ÿ���� ����
    private Transform playerTransform; // �÷��̾��� Transform

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isAggroed)
        {
            // �÷��̾�� ���� ���� �Ÿ��� ���
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > maxAggroDistance)
            {
                // �÷��̾ ���� �Ÿ� �̻����� �־����� ��׷� ���� ��Ȱ��ȭ
                isAggroed = false;
            }
            else
            {
                // ���⿡ ���Ͱ� �÷��̾ �߰��ϴ� ������ �߰��� �� �ֽ��ϴ�.
                // ��: ������ ���¸� �����ϰų� �÷��̾ ���󰡴� ���� ������ ����
            }
        }
    }

    public void TriggerAggro()
    {
        if (!isAggroed)
        {
            // ��׷� ���带 ���
            PlayAggroSound();
            // ��׷� ���¸� Ȱ��ȭ
            isAggroed = true;
        }
    }

    void PlayAggroSound()
    {
        // AudioSource�� �̿��Ͽ� ��׷� ���带 ���
        if (aggroSound != null)
        {
            AudioSource.PlayClipAtPoint(aggroSound, transform.position);
        }
    }
}
