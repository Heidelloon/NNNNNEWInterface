using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    void Update()
    {
        // 목표 지점에 도착했으면 다음 Waypoint로 이동
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            SetDestination();
        }

        else
        {

        }
    }

    void SetDestination()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("Waypoints가 설정되지 않았습니다.");
            return;
        }

        // 다음 Waypoint의 위치를 목표 지점으로 설정
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        // 다음 Waypoint 인덱스 업데이트
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}