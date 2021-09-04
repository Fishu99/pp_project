using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private Gun gunComponent;

    void Awake(){
        EnemyRandomWeapon enemyRandomWeapon = GetComponentInChildren<EnemyRandomWeapon>();
        gunComponent = enemyRandomWeapon.GetGun();
        gunComponent.unlimitedShots = true;
    }

    public void Attack(Vector3 startPosition)
    {
        if (gunComponent != null && gunComponent.CanShoot())
        {
            GetComponent<Animator>().SetTrigger("Attack");
            gunComponent.Shoot(startPosition, transform.forward);
        }
        
    }

    public float GetAnimTypeWeapon() {
        if (gunComponent == null)
            return 0f;

        return gunComponent.type == Gun.Type.Pistol ? 0.5f : 1f;
    }
}
