using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public AudioClip jumpScareSound; // 깜짝 놀래킬 사운드
    public Image jumpScareImage; // 화면을 가리는 UI 이미지

    private AudioSource audioSource;
    private bool jumpScarePlayed = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jumpScareImage.enabled = false; // 시작 시 이미지 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !jumpScarePlayed)
        {
            // 깜짝 놀래키는 사운드 재생
            if (jumpScareSound != null)
            {
                audioSource.PlayOneShot(jumpScareSound);
            }

            // 화면을 가리는 UI 이미지 활성화
            if (jumpScareImage != null)
            {
                jumpScareImage.enabled = true;
            }

            // 일정 시간 후에 화면을 가리는 UI 이미지 비활성화
            Invoke("DeactivateJumpScareImage", 1.5f); // 1.5초 후에 비활성화 (조절 가능)
            
            jumpScarePlayed = true;
        }
    }

    private void DeactivateJumpScareImage()
    {
        if (jumpScareImage != null)
        {
            jumpScareImage.enabled = false;
        }
    }
}