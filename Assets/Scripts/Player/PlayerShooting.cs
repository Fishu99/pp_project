using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for controlling the player's attacks (shooting or stabbing).
/// </summary>
public class PlayerShooting : MonoBehaviour
{

    [SerializeField] Transform startPosition;
    /// <summary>
    /// The currently used weapon
    /// </summary>
    public GameObject activeWeapon;
    /// <summary>
    /// The Gun component of the activeWeapon.
    /// </summary>
    private Gun gunComponent;
    /// <summary>
    /// The Knife component of the activeWeapon
    /// </summary>
    private Knife knifeComponent;
    /// <summary>
    /// The component controlling the player's movement
    /// </summary>
    private PlayerMovement playerMovement;
    /// <summary>
    /// The player's animator.
    /// </summary>
    private Animator animator;
    /// <summary>
    /// Enum describing the type of a weapon.
    /// </summary>
    public enum Weapon { Knife, Gun }
    /// <summary>
    /// Type of the active weapon.
    /// </summary>
    public Weapon weapon;
    /// <summary>
    /// Health of the player
    /// </summary>
    Health health;

    /// <summary>
    /// Gets the PlayerMovement component
    /// </summary>
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        health = GetComponent<Health>();
    }

    /// <summary>
    /// Gets the Animator component.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Checks for player input. If the primary mouse button is pressed, the pplayer attacks. If the R key is pressed, the ammunition is reloaded.
    /// </summary>
    void Update()
    {
        if (!health.IsAlive() || Time.timeScale <= 0.9f)
            return;

        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
    }

    /// <summary>
    /// Sets a weapon as the active weapon.
    /// </summary>
    /// <param name="newWeapon">the weapon to be set as active weapon.</param>
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

    /// <summary>
    /// Gets the ammunition of the active weapon.
    /// </summary>
    /// <returns>the number of the ammunition of the gun if the activeWeapon is a gun. If the activeWeapon is a knife, it returns 0.</returns>
    public int GetAmmunition()
    {
        return gunComponent != null ? gunComponent.GetAmmunition() : 0;
    }

    /// <summary>
    /// Gets the number of shots before the reload is necessary.
    /// </summary>
    /// <returns>the number of shots before the reload is necessary if the activeWeapon is a gun. If the activeWeapon is a knife, it returns 0.</returns>
    public int GetShotsBeforeReload()
    {
        return gunComponent != null ? gunComponent.reloadCounter : 0;
    }

    /// <summary>
    /// Performs an attack using the activeWeapon.
    /// </summary>
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

    /// <summary>
    /// Reloads the current weapon's magazine.
    /// </summary>
    private void Reload()
    {
        if (gunComponent != null)
        {
            gunComponent.Reload();
        }
    }

    /// <summary>
    /// Gets the Gun or Knife component of the activeWeapon.
    /// </summary>
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
