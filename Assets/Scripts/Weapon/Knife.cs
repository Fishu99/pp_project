using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private int healthDamage = 10;
    [SerializeField] private List<string> ignoredTags;

    [SerializeField]
    AudioClip hitSound;

    [SerializeField]
    AudioClip missSound;
 
    private bool isAttacking = false;
    AudioSource audioSource;
    float timerAttacking = 0;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking){
            timerAttacking += Time.deltaTime;
            if(timerAttacking > attackDuration){
                isAttacking = false;
                timerAttacking = 0f;
            }
        }
    }

    public void Attack(Vector3 direction, Vector3 startPosition)
    {
        if (!isAttacking)
        {
            AttackSequence(direction, startPosition);
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void AttackSequence(Vector3 direction, Vector3 startPosition)
    {
        timerAttacking = 0f;
        isAttacking = true;
        DamageHealth(direction, startPosition);
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
            audioSource.PlayOneShot(hitSound);
            Health healthComponent = FindHealthOfHitObject(hitInfo);
            healthComponent.Damage(healthDamage);
        }else{
            audioSource.PlayOneShot(missSound);
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
