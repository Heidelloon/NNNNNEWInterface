using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public AudioClip jumpScareSound; // ��¦ �ų ����
    public Image jumpScareImage; // ȭ���� ������ UI �̹���

    private AudioSource audioSource;
    private bool jumpScarePlayed = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jumpScareImage.enabled = false; // ���� �� �̹��� ��Ȱ��ȭ
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !jumpScarePlayed)
        {
            // ��¦ �Ű�� ���� ���
            if (jumpScareSound != null)
            {
                audioSource.PlayOneShot(jumpScareSound);
            }

            // ȭ���� ������ UI �̹��� Ȱ��ȭ
            if (jumpScareImage != null)
            {
                jumpScareImage.enabled = true;
            }

            // ���� �ð� �Ŀ� ȭ���� ������ UI �̹��� ��Ȱ��ȭ
            Invoke("DeactivateJumpScareImage", 1.5f); // 1.5�� �Ŀ� ��Ȱ��ȭ (���� ����)
            
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