using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A script for controlling the health of the player and the enemies
/// </summary>
public class Health : MonoBehaviour
{
    /// <summary>
    /// Max health value.
    /// </summary>
    [SerializeField] private int maxHealth = 100;
    /// <summary>
    /// Health bar for displaing the health of the character.
    /// </summary>
    [SerializeField] private HealthBarUI healthUI;
    /// <summary>
    /// The audio source to play the hit sound.
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// An audio clip to be played when the health is damaged.
    /// </summary>
    [SerializeField] private AudioClip hitSound;

    /// <summary>
    /// Max health value.
    /// </summary>
    public int MaxHealth { get => maxHealth; }

    /// <summary>
    /// Current health value of the character.
    /// </summary>
    public int CurrentHealth { get; private set; }

    /// <summary>
    /// An event invoked when the character dies.
    /// </summary>
    public UnityEvent onDeath;

    /// <summary>
    /// An event invoked when the character get hit.
    /// </summary>
    public UnityEvent onHit;

    /// <summary>
    /// true if the health was 0 at least once.
    /// </summary>
    bool isDead = false;

    void Start()
    {
        CurrentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Reduces the health of the character, plays the hitSound and updates the healthUI.
    /// If the health is less or equal to 0, the onDeath event is invoked.
    /// </summary>
    /// <param name="health">The health to be subtracted from CurrentHealth.</param>
    public void Damage(int health)
    {
        CurrentHealth -= health;
        onHit.Invoke();
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

    /// <summary>
    /// Restores the health of the character and updates the healthUI.
    /// </summary>
    /// <param name="health">The health to be added to CurrentHealth.</param>
    public void Restore(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
        if (healthUI != null)
            healthUI.SetHealth(CurrentHealth, MaxHealth);
    }

    /// <summary>
    /// Tells if the character is alive. A character is alive if their CurrentHealth is positive.
    /// </summary>
    /// <returns>true if CurrentHealth is positive.</returns>
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
