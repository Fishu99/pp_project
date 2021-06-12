using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShooting : MonoBehaviour
{
    
    [SerializeField] private GameObject knife;
    private GameObject activeWeapon;
    private Gun gunComponent;
    private Knife knifeComponent;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        SetActiveWeapon(knife);
        GetComponentOfActiveWeapon();
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
        //Je¿eli poprzednia broñ to by³ nó¿ to go ukrywamy (rozwi¹zanie tymczasowe)
        if(knifeComponent != null)
        {
            knife.SetActive(false);
        }
        newWeapon.transform.position = playerMovement.weaponGrip.transform.position;
        newWeapon.transform.rotation = transform.rotation;
        newWeapon.transform.SetParent(playerMovement.weaponGrip.transform);
        activeWeapon = newWeapon;
        GetComponentOfActiveWeapon();
    }

    public void SetActiveWeaponKnife()
    {
        knife.SetActive(true);
        SetActiveWeapon(knife);
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
