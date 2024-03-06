using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAggro : MonoBehaviour
{
    public float aggroRadius = 10f; // 어그로 반경
    public float maxAggroDistance = 20f; // 어그로 유지 최대 거리
    public AudioClip aggroSound; // 어그로 사운드

    private bool isAggroed = false; // 어그로 상태를 나타내는 변수
    private Transform playerTransform; // 플레이어의 Transform

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isAggroed)
        {
            // 플레이어와 몬스터 간의 거리를 계산
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > maxAggroDistance)
            {
                // 플레이어가 일정 거리 이상으로 멀어지면 어그로 상태 비활성화
                isAggroed = false;
            }
            else
            {
                // 여기에 몬스터가 플레이어를 추격하는 로직을 추가할 수 있습니다.
                // 예: 몬스터의 상태를 변경하거나 플레이어를 따라가는 등의 동작을 수행
            }
        }
    }

    public void TriggerAggro()
    {
        if (!isAggroed)
        {
            // 어그로 사운드를 재생
            PlayAggroSound();
            // 어그로 상태를 활성화
            isAggroed = true;
        }
    }

    void PlayAggroSound()
    {
        // AudioSource를 이용하여 어그로 사운드를 재생
        if (aggroSound != null)
        {
            AudioSource.PlayClipAtPoint(aggroSound, transform.position);
        }
    }
}
