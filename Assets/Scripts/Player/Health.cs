using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private HealthBarUI healthUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    public int MaxHealth { get => maxHealth; }

    public int CurrentHealth { get; private set; }

    public UnityEvent onDeath;

    bool isDead = false;

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
        if (CurrentHealth <= 0){
            CurrentHealth = 0;
            if(!isDead){
                isDead = true;
                onDeath.Invoke();
            }
        }
        if(healthUI != null)
            healthUI.SetHealth(CurrentHealth, MaxHealth);

        if(hitSound != null && audioSource != null){
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void Restore(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
        if (healthUI != null)
            healthUI.SetHealth(CurrentHealth, MaxHealth);
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
