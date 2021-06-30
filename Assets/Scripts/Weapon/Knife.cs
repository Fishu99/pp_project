using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private int healthDamage = 10;
    [SerializeField] private List<string> ignoredTags;
    private bool isAttacking = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector3 direction, Vector3 startPosition)
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackSequence(direction, startPosition));
        }
    }

    private IEnumerator AttackSequence(Vector3 direction, Vector3 startPosition)
    {
        isAttacking = true;
        DamageHealth(direction, startPosition);
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private void DamageHealth(Vector3 direction, Vector3 startPosition)
    {
        Vector3 origin = startPosition;
        bool wasHit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, attackDistance);
        Debug.DrawRay(origin, direction, Color.white, 1);

        if (wasHit) {
            foreach(string tag in ignoredTags) {
                if(hitInfo.collider.gameObject.tag == tag) {
                    return;
                }
            }

            Health healthComponent = FindHealthOfHitObject(hitInfo);
            healthComponent.Damage(healthDamage);
        }
    }

    private Health FindHealthOfHitObject(RaycastHit hitInfo)
    {
        
        Transform tr = hitInfo.collider.gameObject.transform;
        Health healthComponent = tr.GetComponent<Health>();
        while (tr != null && healthComponent == null)
        {
            tr = tr.parent;
            healthComponent = tr?.GetComponent<Health>();
        }
        return healthComponent;
    }
}
