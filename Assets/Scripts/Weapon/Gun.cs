using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private List<string> ignoreTags;
    [SerializeField] private Sprite gunSprite;
    public Sprite GunSprite { get; private set; }
    public bool unlimitedShots = false;
    private int shotsBeforeReload = 5;
    public int reloadCounter;
    public int ammunition = 0;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;
    public float lightDuration = 0.02f;
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
            if (enableMuzzleflash == true)
            {
                muzzleParticles.Emit(1);
                //Light flash start
                StartCoroutine(MuzzleFlashLight());
            }
            if (enableSparks == true)
            {
                //Emit random amount of spark particles
                sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
            }
            if (enableMuzzleflash == true)
            {
                muzzleParticles.Emit(1);
                //Light flash start
                StartCoroutine(MuzzleFlashLight());
            }
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
        projectileComponent.ignoreTags = ignoreTags;
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

    private IEnumerator MuzzleFlashLight()
    {
        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }
}
