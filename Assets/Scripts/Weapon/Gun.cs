using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    public bool unlimitedShots = false;
    private int shotsBeforeReload = 5;
    public int reloadCounter;
    public PlayerInventory playerInventory;

    void Start()
    {
        Reload();
    }

    
    void Update()
    {
        
    }

    public bool CanShoot()
    {
        return unlimitedShots || (playerInventory.ammunition > 0 && reloadCounter > 0);
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
        playerInventory.ammunition += amount;
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
            playerInventory.ammunition--;
        }
    }
}
