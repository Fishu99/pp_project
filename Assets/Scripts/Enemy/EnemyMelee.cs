using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    private Knife knifeComponent;

    void Start()
    {
        knifeComponent = GetComponentInChildren<Knife>();
    }

    public void Attack(Vector3 startPosition)
    {
        if(knifeComponent != null)
        {
            Vector3 direction = transform.forward;
            knifeComponent.Attack(direction, startPosition);
        }
    }
}
