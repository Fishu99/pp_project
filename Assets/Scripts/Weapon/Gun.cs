using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private List<string> ignoreTags;
    [SerializeField] private Sprite gunSprite;

    [SerializeField] float timeToNextShot = 1f;

    AudioSource audioSource;

    public enum Type {Pistol, Rifle}
    public Type type;
    public Sprite GunSprite {
        get{
            return gunSprite;
        } 
    }
    public bool unlimitedShots = false;
    public int shotsBeforeReload = 5;
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


    bool isShooting = false;
    float timerToShooting = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Reload();
    }

    
    void Update()
    {
        if(isShooting){
            timerToShooting += Time.deltaTime;
            if(timerToShooting > timeToNextShot){
                timerToShooting = 0;
                isShooting = false;
            }
        }else{
            timerToShooting = 0;
        }
    }

    public bool CanShoot()
    {
        return (unlimitedShots || (ammunition > 0 && reloadCounter > 0)) && !isShooting;
    }

    public void Shoot(Vector3 startPosition, Vector3 direction)
    {

        if (CanShoot())
        {
            FireProjectile(startPosition, direction);
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
            isShooting = true;
            timerToShooting = 0;
            if(audioSource)
                audioSource.Play();
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

    public void AddIgnoreTag(string tag){
        if(!ignoreTags.Contains(tag))
            ignoreTags.Add(tag);
    }

    public void DeleteIgnoreTag(string tag){
        if(ignoreTags.Contains(tag))
            ignoreTags.Remove(tag);
    }

    private void FireProjectile(Vector3 startPosition, Vector3 direction)
    {
        Vector3 position = muzzle.transform.position;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = startPosition;
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
