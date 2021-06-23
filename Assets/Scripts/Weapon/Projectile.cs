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

    private void OnCollisionEnter(Collision collision)
    {
        foreach(string tag in ignoreTags) {
            //Debug.Log(collision.gameObject.tag + " == " + tag + " " + (collision.gameObject.tag == tag));
            if(collision.gameObject.tag == tag) {
                return;
            }

        }

        DamageHealth(collision);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }

    private void DamageHealth(Collision collision)
    {
        Health healthComponent = FindHealthOfHitObject(collision);
        if(healthComponent != null)
        {
            healthComponent.Damage(healthDamage);
        }
    }

    private Health FindHealthOfHitObject(Collision collision)
    {
        Transform tr = collision.gameObject.transform;
        Health healthComponent = tr.GetComponent<Health>();
        while(tr != null && healthComponent == null)
        {
            tr = tr.parent;
            healthComponent = tr?.GetComponent<Health>();
        }
        return healthComponent;
    }
}
