using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private new Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public void Shoot()
    {
        rigidbody.velocity = transform.forward * speed;
    }
}
