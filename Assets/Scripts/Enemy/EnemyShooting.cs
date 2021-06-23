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

    public void Attack()
    {
        if (gunComponent != null)
        {
            gunComponent.Shoot();
        }
    }
}
