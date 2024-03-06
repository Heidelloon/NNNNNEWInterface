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
        // ��ǥ ������ ���������� ���� Waypoint�� �̵�
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
            Debug.LogError("Waypoints�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� Waypoint�� ��ġ�� ��ǥ �������� ����
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        // ���� Waypoint �ε��� ������Ʈ
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}