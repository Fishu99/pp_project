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
    public enum Weapon { Knife, Gun }
    public Weapon weapon;

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
        newWeapon.transform.rotation = playerMovement.weaponGrip.transform.rotation;
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
                animator.SetTrigger("Attack");
            }
            gunComponent.Shoot();
        }
        else if(knifeComponent != null)
        {
            Vector3 direction = transform.forward;
            knifeComponent.Attack(direction);
            animator.SetTrigger("Attack");
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
        if(gunComponent != null)
        {
            weapon = Weapon.Gun;
            if(Equals(gunComponent.type, Gun.Type.Pistol))
            {
                animator.SetBool("hasPistol", true);
                animator.SetBool("hasRifle", false);
                animator.SetBool("hasKnife", false);
            }
            else if (Equals(gunComponent.type, Gun.Type.Rifle))
            {
                animator.SetBool("hasRifle", true);
                animator.SetBool("hasPistol", false);
                animator.SetBool("hasKnife", false);
            }
        }
        knifeComponent = activeWeapon.GetComponent<Knife>();
        if (knifeComponent != null)
        {
            weapon = Weapon.Knife;
            if(animator != null)
            {
                animator.SetBool("hasKnife", true);
                animator.SetBool("hasPistol", false);
                animator.SetBool("hasRifle", false);
            }
        }
    }
}
