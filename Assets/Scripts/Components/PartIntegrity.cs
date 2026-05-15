using UnityEngine;
using System;

public class PartIntegrity : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Game Over Settings")]
    [Tooltip("Centang ini jika hancurnya part ini (HP 0) akan langsung memicu Game Over (misal: Balon Utama)")]
    public bool isCriticalPart = false; 

    public event Action<int, int> OnHealthChanged;

    private bool isDestroyed = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        currentHP -= damage;
        
        if (currentHP <= 0)
        {
            currentHP = 0;
            PartDestroyed();
        }

        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void RepairPart(int healAmount)
    {
        if (currentHP >= maxHP) return;

        currentHP += healAmount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        isDestroyed = false;

        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    private void PartDestroyed()
    {
        isDestroyed = true;
        Debug.Log(gameObject.name + " telah hancur!");

        if (isCriticalPart)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("Critical Part hancur! Memulai transisi Game Over...");
        
        if (GlobalTransition.Instance != null)
        {
            GlobalTransition.Instance.FadeOutAndLoadScene("DemoGameOver");
        }
        else
        {
            Debug.LogWarning("GlobalTransition tidak ditemukan di Scene!");
        }
    }
}