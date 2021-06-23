using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShooting : MonoBehaviour
{
    public GameObject activeWeapon;
    private Gun gunComponent;
    private Knife knifeComponent;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
    }

    public void SetActiveWeapon(GameObject newWeapon)
    {
        if (playerMovement == null)
            Debug.Log("null");
        newWeapon.transform.position = playerMovement.weaponGrip.transform.position;
        newWeapon.transform.rotation = transform.rotation * Quaternion.Euler(0,-12,0);
        newWeapon.transform.SetParent(playerMovement.weaponGrip.transform);
        activeWeapon = newWeapon;
        newWeapon.SetActive(true);
        GetComponentOfActiveWeapon();
    }

    public int GetAmmunition()
    {
        return gunComponent != null ? gunComponent.ammunition : 0;
    }

    public int GetShotsBeforeReload()
    {
        return gunComponent != null ? gunComponent.reloadCounter : 0;
    }

    

    private void Attack()
    {
        if (gunComponent != null)
        {
            if(!Equals(gunComponent.ammunition,0))
            {
                animator.SetTrigger("Shoot");
            }
            gunComponent.Shoot();
        }
        else if(knifeComponent != null)
        {
            Vector3 direction = transform.forward;
            knifeComponent.Attack(direction);
        }
    }

    private void Reload()
    {
        if (gunComponent != null)
        {
            gunComponent.Reload();
        }
    }

    private void GetComponentOfActiveWeapon()
    {
        gunComponent = activeWeapon.GetComponent<Gun>();
        knifeComponent = activeWeapon.GetComponent<Knife>();
    }
}
