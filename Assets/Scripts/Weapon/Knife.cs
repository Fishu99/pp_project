using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for the knife.
/// </summary>
public class Knife : MonoBehaviour
{
    /// <summary>
    /// Duration of the attack (the minimal time between two attacks).
    /// </summary>
    [SerializeField] private float attackDuration = 0.5f;

    /// <summary>
    /// The distance of the attack.
    /// </summary>
    [SerializeField] private float attackDistance = 0.5f;

    /// <summary>
    /// The health damage caused by the knife.
    /// </summary>
    [SerializeField] private int healthDamage = 10;

    /// <summary>
    /// List of tags of objects ignored by the knife.
    /// </summary>
    [SerializeField] private List<string> ignoredTags;

    /// <summary>
    /// The sound played after the attack if the knife hits something.
    /// </summary>
    [SerializeField]
    AudioClip hitSound;

    /// <summary>
    /// The sound played after the attack if the doesn't hit anything.
    /// </summary>
    [SerializeField]
    AudioClip missSound;

    /// <summary>
    /// Is true after each shot for the time timeToNextShot.
    /// </summary>
    private bool isAttacking = false;

    /// <summary>
    /// AudioSource used for playing sound effects.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// A timer counting time after an attack.
    /// </summary>
    float timerAttacking = 0;
    
    /// <summary>
    /// Initializes some component references.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Measures the time after each shot during which the knife cannot attack.
    /// </summary>
    void Update()
    {
        if(isAttacking){
            timerAttacking += Time.deltaTime;
            if(timerAttacking > attackDuration){
                isAttacking = false;
                timerAttacking = 0f;
            }
        }
    }

    /// <summary>
    /// Attacks in the given position and direction.
    /// The attack is made by casting a ray of length attackDistance from the point startPosition
    /// in the direction direction. If the ray hits any object with Health component,
    /// the healthDamage is subracted from the hit object's health.
    /// </summary>
    /// <param name="direction">The direction of the attack.</param>
    /// <param name="startPosition">The point where the attack begins.</param>
    public void Attack(Vector3 direction, Vector3 startPosition)
    {
        if (!isAttacking)
        {
            AttackSequence(direction, startPosition);
        }
    }

    /// <summary>
    /// Gets the value indicating if the knife is attacking.
    /// </summary>
    /// <returns>true if the knife is attacking.</returns>
    public bool IsAttacking()
    {
        return isAttacking;
    }

    /// <summary>
    /// Resets atack timer and performs the attack.</summary>
    /// <param name="direction">The direction of the attack.</param>
    /// <param name="startPosition">The point where the attack begins.</param>
    private void AttackSequence(Vector3 direction, Vector3 startPosition)
    {
        timerAttacking = 0f;
        isAttacking = true;
        DamageHealth(direction, startPosition);
    }

    /// <summary>
    /// Casts a ray of length attackDistance from the point startPosition
    /// in the direction direction. If the ray hits any object with Health component,
    /// the healthDamage is subracted from the hit object's health.
    /// </summary>
    /// <param name="direction">The direction of the attack.</param>
    /// <param name="startPosition">The point where the attack begins.</param>
    private void DamageHealth(Vector3 direction, Vector3 startPosition)
    {
        Vector3 origin = startPosition;
        bool wasHit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, attackDistance);
        Debug.DrawRay(origin, direction, Color.white, 1);

        if (wasHit) {
            foreach(string tag in ignoredTags) {
                if(hitInfo.collider.gameObject.tag == tag) {
                    return;
                }
            }
            audioSource.PlayOneShot(hitSound);
            Health healthComponent = FindHealthOfHitObject(hitInfo);
            healthComponent.Damage(healthDamage);
        }else{
            audioSource.PlayOneShot(missSound);
        }
    }

    /// <summary>
    /// Finds the health component of the object hit by the knife.
    /// If the object does not have a Health component, the method checks recursively the parent object
    /// until the health component is found or the parent is null.
    /// </summary>
    /// <param name="hitInfo">The raycast hit structure</param>
    /// <returns>the Health component of the object or of one of its parent objects. If no parent has a Health component, null is returned.</returns>
    private Health FindHealthOfHitObject(RaycastHit hitInfo)
    {
        
        Transform tr = hitInfo.collider.gameObject.transform;
        Health healthComponent = tr.GetComponent<Health>();
        while (tr != null && healthComponent == null)
        {
            tr = tr.parent;
            healthComponent = tr?.GetComponent<Health>();
        }
        return healthComponent;
    }
}
