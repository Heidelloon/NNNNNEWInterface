using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHealth.ToString();
        }
    }

    public void AddHealth(int recoverAmount)
    {
        currentHealth += recoverAmount;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {

        Debug.Log("���� x");

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }

        UpdateHealthUI();
    }

    private void GameOver()
    {
        // ���� ���� �� ������ ���� �߰�
        Debug.Log("Game Over");
    }
}