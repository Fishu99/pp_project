using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private Gun gunComponent;

    void Start()
    {
        gunComponent = GetComponentInChildren<Gun>();
        gunComponent.unlimitedShots = true;
    }

    public void Attack(Vector3 startPosition)
    {
        if (gunComponent != null)
        {
            gunComponent.Shoot(startPosition, transform.forward);
        }
    }
}
