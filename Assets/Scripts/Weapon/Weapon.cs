using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    public int ammunition = 20;
    private int shotsBeforeReload = 5;
    public int reloadCounter;

    void Start()
    {
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanShoot()
    {
        return ammunition > 0 && reloadCounter > 0;
    }

    public bool Shoot()
    {
        bool canShoot = CanShoot();
        if (canShoot)
        {
            FireProjectile();
            reloadCounter--;
            ammunition--;
        }
        return canShoot;
    }

    public void Reload()
    {
        reloadCounter = shotsBeforeReload;
    }

    public void AddAmmunition(int amount)
    {
        ammunition += amount;
    }

    private void FireProjectile()
    {
        Vector3 position = muzzle.transform.position;
        Vector3 direction = transform.forward;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = position;
        projectile.transform.rotation = Quaternion.FromToRotation(projectile.transform.forward, direction);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.Shoot();
    }
}
