using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // 기존 변수들
    public Transform player; // 플레이어 Transform
    public float speed = 5f; // 이동 속도
    public float range = 10f; // 추격 범위
    public float chaseCooldown = 5f; // 추격 쿨다운 시간

    private bool canChase = true; // 추격 가능 여부
    private float chaseTimer = 0f; // 추격 쿨다운 타이머

    // 스턴 효과에 대한 새로운 변수들
    public float stunDuration = 2f; // 스턴 효과의 지속 시간
    private bool isStunned = false; // 스턴 상태 여부
    private float stunTimer = 0f; // 스턴 효과 타이머

    // 순찰에 대한 기존 변수들
    public Transform[] waypoints; // 순찰 지점 배열
    private int currentWaypointIndex = 0; // 현재 순찰 지점 인덱스
    private NavMeshAgent navMeshAgent; // NavMeshAgent

    // 어그로에 대한 기존 변수들
    public float aggroRadius = 10f; // 어그로 반경
    public float maxAggroDistance = 20f; // 최대 어그로 거리
    public AudioClip aggroSound; // 어그로 사운드

    private bool isAggroed = false; // 어그로 상태 여부
    private Transform playerTransform; // 플레이어 Transform

    void Start()
    {
        // 참조 설정
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination(); // 초기 목적지 설정
    }

    void Update()
    {
        // 스턴 상태인 경우 이동 업데이트를 하지 않음
        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= stunDuration)
            {
                isStunned = false;
                canChase = true; // 스턴 지속 시간이 끝나면 추격 가능 상태로 변경
                stunTimer = 0f;
            }
        }
        else
        {
            // 추격 및 쿨다운 업데이트 로직
            if (canChase)
            {
                // 어그로 확인
                if (!isAggroed)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer < aggroRadius)
                    {
                        TriggerAggro(); // 어그로 트리거
                    }
                }

                // 추격 로직
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance <= range)
                {
                    transform.LookAt(player);
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
            }
            else
            {
                // 쿨다운 로직
                chaseTimer += Time.deltaTime;
                if (chaseTimer >= chaseCooldown)
                {
                    canChase = true;
                    chaseTimer = 0f;
                }
            }

            // 순찰 로직
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
            Debug.LogError("Waypoints가 설정되지 않았습니다.");
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
            isStunned = true; // 스턴 효과 적용
            stunTimer = 0f; // 스턴 타이머 초기화
            Debug.Log("충돌");
        }
        else if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // 플레이어에게 데미지 입히기
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
