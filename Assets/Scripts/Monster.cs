using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // ���� ������
    public Transform player; // �÷��̾� Transform
    public float speed = 5f; // �̵� �ӵ�
    public float range = 10f; // �߰� ����
    public float chaseCooldown = 5f; // �߰� ��ٿ� �ð�

    private bool canChase = true; // �߰� ���� ����
    private float chaseTimer = 0f; // �߰� ��ٿ� Ÿ�̸�

    // ���� ȿ���� ���� ���ο� ������
    public float stunDuration = 2f; // ���� ȿ���� ���� �ð�
    private bool isStunned = false; // ���� ���� ����
    private float stunTimer = 0f; // ���� ȿ�� Ÿ�̸�

    // ������ ���� ���� ������
    public Transform[] waypoints; // ���� ���� �迭
    private int currentWaypointIndex = 0; // ���� ���� ���� �ε���
    private NavMeshAgent navMeshAgent; // NavMeshAgent

    // ��׷ο� ���� ���� ������
    public float aggroRadius = 10f; // ��׷� �ݰ�
    public float maxAggroDistance = 20f; // �ִ� ��׷� �Ÿ�
    public AudioClip aggroSound; // ��׷� ����

    private bool isAggroed = false; // ��׷� ���� ����
    private Transform playerTransform; // �÷��̾� Transform

    void Start()
    {
        // ���� ����
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination(); // �ʱ� ������ ����
    }

    void Update()
    {
        // ���� ������ ��� �̵� ������Ʈ�� ���� ����
        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= stunDuration)
            {
                isStunned = false;
                canChase = true; // ���� ���� �ð��� ������ �߰� ���� ���·� ����
                stunTimer = 0f;
            }
        }
        else
        {
            // �߰� �� ��ٿ� ������Ʈ ����
            if (canChase)
            {
                // ��׷� Ȯ��
                if (!isAggroed)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer < aggroRadius)
                    {
                        TriggerAggro(); // ��׷� Ʈ����
                    }
                }

                // �߰� ����
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance <= range)
                {
                    transform.LookAt(player);
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
            }
            else
            {
                // ��ٿ� ����
                chaseTimer += Time.deltaTime;
                if (chaseTimer >= chaseCooldown)
                {
                    canChase = true;
                    chaseTimer = 0f;
                }
            }

            // ���� ����
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                SetDestination();
            }
        }
    }

    void SetDestination()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("Waypoints�� �������� �ʾҽ��ϴ�.");
            return;
        }

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hammer"))
        {
            canChase = false;
            isStunned = true; // ���� ȿ�� ����
            stunTimer = 0f; // ���� Ÿ�̸� �ʱ�ȭ
            Debug.Log("�浹");
        }
        else if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // �÷��̾�� ������ ������
            }
        }
    }

    public void TriggerAggro()
    {
        if (!isAggroed)
        {
            PlayAggroSound();
            isAggroed = true;
        }
    }

    void PlayAggroSound()
    {
        if (aggroSound != null)
        {
            AudioSource.PlayClipAtPoint(aggroSound, transform.position);
        }
    }
}
