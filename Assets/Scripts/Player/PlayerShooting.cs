using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShooting : MonoBehaviour
{

    [SerializeField]
    Transform startPosition;

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
        if (Input.GetMouseButton(0))
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
            if(gunComponent.CanShoot())
            {
                animator.SetTrigger("Attack");
                gunComponent.Shoot(startPosition.position, transform.forward);
            }
            
        }
        else if(knifeComponent != null && !knifeComponent.IsAttacking())
        {
            Vector3 direction = transform.forward;
            knifeComponent.Attack(direction, startPosition.position);
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
                animator.SetFloat("Weapon", 0.5f);
            }
            else if (Equals(gunComponent.type, Gun.Type.Rifle))
            {
                animator.SetFloat("Weapon", 1f);
            }
        }
        knifeComponent = activeWeapon.GetComponent<Knife>();
        if (knifeComponent != null)
        {
            weapon = Weapon.Knife;
            if(animator != null)
            {
                animator.SetFloat("Weapon", 0f);
            }
        }
    }
}
