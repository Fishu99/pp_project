using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int healthDamage = 10;

    public List<string> ignoreTags;
    
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in ignoreTags) {
            //Debug.Log(collision.gameObject.tag + " == " + tag + " " + (collision.gameObject.tag == tag));
            if(other.gameObject.tag == tag) {
                return;
            }

        }

        DamageHealth(other);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }

    private void DamageHealth(Collider collider)
    {
        Health healthComponent = FindHealthOfHitObject(collider);
        if(healthComponent != null)
        {
            healthComponent.Damage(healthDamage);
        }
    }

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
