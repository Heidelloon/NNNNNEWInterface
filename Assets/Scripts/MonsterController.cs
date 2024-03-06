using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform player;
    public Transform hammer; // 해머의 위치를 가져오기 위한 변수
    public float speed = 5f;
    public float range = 10f;
    public float chaseCooldown = 5f; // 쫒아가기 재개까지의 대기 시간

    private bool canChase = true; // 쫒아가기 가능 여부를 나타내는 변수
    private float chaseTimer = 0f; // 쫒아가기 대기 타이머

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
            // 쫒아가기 가능 여부가 false인 경우, 쿨다운 타이머를 업데이트
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseCooldown)
            {
                // 쿨다운이 끝나면 다시 쫒아가기 가능하도록 설정
                canChase = true;
                chaseTimer = 0f; // 타이머 초기화
            }
        }
    }

    // 몬스터와 해머 사이의 충돌을 감지하는 메서드
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hammer"))
        {
            // 충돌한 오브젝트가 해머일 때 쫒아가지 않도록 설정
            canChase = false;
        }
    }
}
