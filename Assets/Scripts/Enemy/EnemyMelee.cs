using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private Knife knifeComponent;
    private Animator animator;
    void Start()
    {
        knifeComponent = GetComponentInChildren<Knife>();
        animator = GetComponent<Animator>();
    }

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
