using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for the guns (weapons that fire projectiles).
/// </summary>
public class Gun : MonoBehaviour
{
    /// <summary>
    /// The prefab of the projectile shot by the gun.
    /// </summary>
    [SerializeField] private GameObject projectilePrefab;

    /// <summary>
    /// The point where the projectiles appear.
    /// </summary>
    [SerializeField] private GameObject muzzle;

    /// <summary>
    /// Tags to be ignored by the projectiles shot from the gun.
    /// </summary>
    [SerializeField] private List<string> ignoreTags;

    /// <summary>
    /// The sprite of the gun displayed in the UI.
    /// </summary>
    [SerializeField] private Sprite gunSprite;

    /// <summary>
    /// The minimal time between two shots.
    /// </summary>
    [SerializeField] float timeToNextShot = 1f;

    /// <summary>
    /// The source of the gun sounds.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Enum describing the type of the gun.
    /// </summary>
    public enum Type {Pistol, Rifle}

    /// <summary>
    /// The type of the gun.
    /// </summary>
    public Type type;

    /// <summary>
    /// Gets the gun sprite.
    /// </summary>
    public Sprite GunSprite {
        get{
            return gunSprite;
        } 
    }

    /// <summary>
    /// If set to true, the gun has unlimited ammunition and does not need to be reloaded.
    /// </summary>
    public bool unlimitedShots = false;

    /// <summary>
    /// The amount of ammunition which will be available after each reload. In other words, it is the size of the gun's magazine.
    /// </summary>
    public int shotsBeforeReload = 5;

    /// <summary>
    /// The amount of ammunition which can be shot before the gun needs to be reloaded. It is decremented after each shot.
    /// </summary>
    public int reloadCounter;
    [SerializeField]
    private int ammunition = 0;
    [SerializeField]
    private float difficultyAmmunition = 1f;

    /// <summary>
    /// Enables muzzle flash effect.
    /// </summary>
    public bool enableMuzzleflash = true;

    /// <summary>
    /// Particles for the muzzle flash effect.
    /// </summary>
    public ParticleSystem muzzleParticles;

    /// <summary>
    /// Enables sparks effect.
    /// </summary>
    public bool enableSparks = true;

    /// <summary>
    /// Particles for the sparks effect.
    /// </summary>
    public ParticleSystem sparkParticles;

    /// <summary>
    /// Minimal sprak emission.
    /// </summary>
    public int minSparkEmission = 1;

    /// <summary>
    /// Maximal spark emission.
    /// </summary>
    public int maxSparkEmission = 7;

    /// <summary>
    /// The light present during the flash effect.
    /// </summary>
    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;

    /// <summary>
    /// The duration of muzzle flash light.
    /// </summary>
    public float lightDuration = 0.02f;

    /// <summary>
    /// Is true after each shot for the time timeToNextShot.
    /// </summary>
    bool isShooting = false;

    /// <summary>
    /// A timer counting time after a shot.
    /// </summary>
    float timerToShooting = 0;

    /// <summary>
    /// Initializes the components and sets random initial ammunition
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ammunition = Random.Range(0, 20);
        Reload();
    }

    /// <summary>
    /// Measures the time after each shot during which the gun cannot shoot.
    /// </summary>
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

    /// <summary>
    /// Indicates if the gun can shoot a projectile.
    /// </summary>
    /// <returns>true if the gun can shoot.</returns>
    public bool CanShoot()
    {
        return (unlimitedShots || (ammunition > 0 && reloadCounter > 0)) && !isShooting;
    }

    /// <summary>
    /// Shoots a projectile so that it appears at the given position and moves in the given direction. Visual and sound effects are played.
    /// The pojectile is shot only if CanShoot returns true.
    /// After the shot the reloadCounter and ammunition are decremented.
    /// </summary>
    /// <param name="startPosition">The position where the projectile will be instantiated.</param>
    /// <param name="direction">The direction in which the projectile will be shot.</param>
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

    /// <summary>
    /// Reloads the gun.
    /// </summary>
    public void Reload()
    {
        reloadCounter = shotsBeforeReload;
    }

    /// <summary>
    /// Adds ammunition to the gun.
    /// </summary>
    /// <param name="amount">the amount of ammunition to add.</param>
    public void AddAmmunition(int amount)
    {
        ammunition += (int)(amount * difficultyAmmunition);
    }

    public int GetAmmunition() {
        return ammunition;
    }

    public float GetDifficultyAmmunition() {
        return difficultyAmmunition;
    }

    /// <summary>
    /// Adds a tag to be ignored by the fired projectile.
    /// </summary>
    /// <param name="tag">The tag to be added.</param>
    public void AddIgnoreTag(string tag){
        if(!ignoreTags.Contains(tag))
            ignoreTags.Add(tag);
    }

    /// <summary>
    /// Removes a tag from the tags ignored by the fired projectiles.
    /// </summary>
    /// <param name="tag">The tag to remove.</param>
    public void DeleteIgnoreTag(string tag){
        if(ignoreTags.Contains(tag))
            ignoreTags.Remove(tag);
    }

    /// <summary>
    /// Instantiates the projectile prefab at the given position and shoots in the given direction.
    /// </summary>
    /// <param name="startPosition">The position where the projectile will be instantiated.</param>
    /// <param name="direction">The direction in which the projectile will be shot.</param>
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

    /// <summary>
    /// Decrements the number of ammunition and the number of shots after the reload
    /// if unlimitedShots is not true.
    /// </summary>
    private void DecrementCounters()
    {
        if (!unlimitedShots)
        {
            reloadCounter--;
            ammunition--;
        }
    }

    /// <summary>
    /// A coroutine for controlling the light at the muzzle after the shot.
    /// </summary>
    /// <returns>a coroutine controlling the light at the muzzle after the shot</returns>
    private IEnumerator MuzzleFlashLight()
    {
        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }
}
