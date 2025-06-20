using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private HealthUI healthUI;

    private void Awake()
    {
        healthUI = GetComponentInChildren<HealthUI>();
    }

    public void InitializeMaxHealth(int maxHealth)
    {
        healthUI.Initialize(maxHealth);
    }

    public void SetHealth(int currentHealth)
    {
        healthUI.SetHealth(currentHealth);
    }
}