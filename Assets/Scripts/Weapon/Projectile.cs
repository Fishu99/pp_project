using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for a projectile. Projectiles are shot from a Gun.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Speed of the projectile.
    /// </summary>
    [SerializeField] private float speed = 10f;

    /// <summary>
    /// Health damage caused by the projectile.
    /// </summary>
    [SerializeField] private int healthDamage = 10;

    /// <summary>
    /// An effect played when a projectile hits something.
    /// </summary>
    [SerializeField] private GameObject effect;

    /// <summary>
    /// Time when the projectile disappears after being fired.
    /// </summary>
    [SerializeField] private float timeToDie = 3f;

    /// <summary>
    /// Tags of an object ignored by the projectile.
    /// </summary>
    public List<string> ignoreTags;
    
    /// <summary>
    /// Projectile's Rigidbody component.
    /// </summary>
    private new Rigidbody rigidbody;

    /// <summary>
    /// Timer measuring the lifetime of the projectile.
    /// </summary>
    private float timer;

    /// <summary>
    /// Initializes the component references and the timer.
    /// </summary>
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        timer = 0;
    }

    /// <summary>
    /// Updates the timer. If the timer shows a value larger than timeToDie, the projectile destroys itself.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeToDie){
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Executed when other collider enters the projetile's trigger collider.
    /// If the other collider's object has a tag from ignoreTags the collision is ignored and the projectile keeps moving forward.
    /// Otherwise, the projectile is destroyed.
    /// If the other collider's object has a Health component (and its tag is not in ignoreTags), the health is damaged.
    /// </summary>
    /// <param name="other">The collider which entered the projectile's collider.</param>
    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in ignoreTags) {
            if(other.gameObject.tag == tag) {
                return;
            }

        }

        DamageHealth(other);
        Destroy(gameObject);
    }

    /// <summary>
    /// Shoots the projectile in its forward direction.
    /// </summary>
    public void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }

    /// <summary>
    /// Damages the health of the object hit by the projectile if the other object has a Health component.
    /// </summary>
    /// <param name="collider">The collider of the other object.</param>
    private void DamageHealth(Collider collider)
    {
        Health healthComponent = FindHealthOfHitObject(collider);
        BulletEffect be = null;
        if(effect != null){
            GameObject currentEffect = Instantiate(effect,transform.position,Quaternion.identity);
            be = currentEffect.GetComponent<BulletEffect>();
        }

        if(healthComponent != null){
            healthComponent.Damage(healthDamage);
            if(be != null){
                be.Init(true);
            }
        }else if(be != null){
            be.Init(false);
        }
    }

    /// <summary>
    /// Finds the health component of the object hit by the projectile.
    /// If the object does not have a Health component, the method checks recursively the parent object
    /// until the health component is found or the parent is null.
    /// </summary>
    /// <param name="collider">The collider of the hit object</param>
    /// <returns>the Health component of the object or one of its parent objects. If no parent has a Health component, null is returned.</returns>
    private Health FindHealthOfHitObject(Collider collider)
    {
        Transform tr = collider.gameObject.transform;
        Health healthComponent = tr.GetComponent<Health>();
        while(tr != null && healthComponent == null)
        {
            tr = tr.parent;
            healthComponent = tr?.GetComponent<Health>();
        }
        return healthComponent;
    }
}
