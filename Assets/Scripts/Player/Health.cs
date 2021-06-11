using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth { get => maxHealth; }

    public int CurrentHealth { get; private set; }

    void Start()
    {
        CurrentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void Damage(int health)
    {
        CurrentHealth -= health;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    public void Restore(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
