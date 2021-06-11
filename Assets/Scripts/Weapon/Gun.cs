using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    public bool unlimitedShots = false;
    public int ammunition = 20;
    private int shotsBeforeReload = 5;
    public int reloadCounter;

    void Start()
    {
        Reload();
    }

    
    void Update()
    {
        
    }

    public bool CanShoot()
    {
        return unlimitedShots || (ammunition > 0 && reloadCounter > 0);
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            FireProjectile();
            DecrementCounters();
        }
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

    private void DecrementCounters()
    {
        if (!unlimitedShots)
        {
            reloadCounter--;
            ammunition--;
        }
    }
}
