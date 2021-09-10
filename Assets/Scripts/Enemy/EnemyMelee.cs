using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for a melee enemy (attacking with a knife).
/// </summary>
public class EnemyMelee : MonoBehaviour{

    [SerializeField]
    float timeToHit = 0.8f;

    /// <summary>
    /// The knife used for attacking.
    /// </summary>
    private Knife knifeComponent;

    /// <summary>
    /// The enemy's animator.
    /// </summary>
    private Animator animator;

    float timer = 0f;

    /// <summary>
    /// Initializes the components.
    /// </summary>
    void Start()
    {
        knifeComponent = GetComponentInChildren<Knife>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Attacks using the knife starting from the specified position in the forward direction.
    /// </summary>
    /// <param name="startPosition">The position from which the attack begins.</param>
    public void Attack(Vector3 startPosition)
    {
        if(knifeComponent != null)
        {
            Vector3 direction = transform.forward;
            knifeComponent.Attack(direction, startPosition);
            animator.SetTrigger("Attack");
        }
    }
}
