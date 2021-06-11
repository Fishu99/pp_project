using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private Knife knife;
    [SerializeField] private float attackPeriod = 1;
    private Health health;

    void Start()
    {
        gun.unlimitedShots = true;
        health = GetComponent<Health>();
        StartCoroutine(AttackRepeatedly());
    }

    void Update()
    {
        if (!health.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AttackRepeatedly()
    {
        while (true)
        {
            gun.Shoot();
            Vector3 direction = knife.gameObject.transform.forward;
            knife.Attack(direction);
            yield return new WaitForSeconds(attackPeriod);
        }
    }
}
