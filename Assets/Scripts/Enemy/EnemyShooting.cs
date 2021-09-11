using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for a shooting enemy (attacking with a gun).
/// </summary>
public class EnemyShooting : MonoBehaviour
{
    /// <summary>
    /// Enemy's gun used for attacks.
    /// </summary>
    private Gun gunComponent;

    /// <summary>
    /// Initializes the enemy's gun.
    /// </summary>
    void Awake(){
        EnemyRandomWeapon enemyRandomWeapon = GetComponentInChildren<EnemyRandomWeapon>();
        gunComponent = enemyRandomWeapon.GetGun();
        gunComponent.unlimitedShots = true;
    }

    /// <summary>
    /// Shoots a projectile from the gun from the given position.
    /// </summary>
    /// <param name="startPosition">The position where the projectile appears.</param>
    public void Attack(Vector3 startPosition)
    {
        if (gunComponent != null && gunComponent.CanShoot())
        {
            GetComponent<Animator>().SetTrigger("Attack");
            gunComponent.Shoot(startPosition, transform.forward);
        }
        
    }

    /// <summary>
    /// Returns a number describing the type of the weapon for animation purposes.
    /// </summary>
    /// <returns>0 if the enemy has no gun, 0.5f if the gun is a Pistol, 1f otherwise.</returns>
    public float GetAnimTypeWeapon() {
        if (gunComponent == null)
            return 0f;

        return gunComponent.type == Gun.Type.Pistol ? 0.5f : 1f;
    }
}
