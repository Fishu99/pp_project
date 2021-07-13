using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int healthDamage = 10;
    [SerializeField] private GameObject effect;
    [SerializeField] private float timeToDie = 3f;

    public List<string> ignoreTags;
    
    private new Rigidbody rigidbody;

    private float timer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeToDie){
            Destroy(gameObject);
        }
    }

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

    public void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }

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
