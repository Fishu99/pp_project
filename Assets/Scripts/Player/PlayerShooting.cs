using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject weapon;
    public Gun weaponComponent;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        weaponComponent = weapon.GetComponent<Gun>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public int GetAmmunition()
    {
        return weaponComponent.ammunition;
    }

    public int GetShotsBeforeReload()
    {
        return weaponComponent.reloadCounter;
    }

    private void Shoot()
    {
        weaponComponent.Shoot();
    }

    private void Reload()
    {
        weapon.GetComponent<Gun>().Reload();
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerShooting : MonoBehaviour
//{
//    [SerializeField] private GameObject gun;
//    [SerializeField] private GameObject knife;
//    private GameObject activeWeapon;
//    private Gun gunComponent;
//    private Knife knifeComponent;
//    private PlayerMovement playerMovement;
//    void Start()
//    {
//        playerMovement = GetComponent<PlayerMovement>();
//        SetActiveWeapon(gun);
//        GetComponentOfActiveWeapon();
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Attack();
//        }
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Reload();
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            SetActiveWeapon(gun);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            SetActiveWeapon(knife);
//        }
//    }

//    public void SetActiveWeapon(GameObject newWeaponPrefab)
//    {
//        if(activeWeapon != null)
//        {
//            Destroy(activeWeapon);
//        }
//        GameObject newWeapon = Instantiate(newWeaponPrefab);
//        newWeapon.transform.position = playerMovement.weaponGrip.transform.position;
//        newWeapon.transform.rotation = transform.rotation;
//        newWeapon.transform.SetParent(playerMovement.weaponGrip.transform);
//        activeWeapon = newWeapon;
//        GetComponentOfActiveWeapon();
//    }

//    public int GetAmmunition()
//    {
//        return gunComponent != null ? gunComponent.ammunition : 0;
//    }

//    public int GetShotsBeforeReload()
//    {
//        return gunComponent != null ? gunComponent.reloadCounter : 0;
//    }

//    private void Attack()
//    {
//        if (gunComponent != null)
//        {
//            gunComponent.Shoot();
//        }
//        else if(knifeComponent != null)
//        {
//            Vector3 direction = transform.forward;
//            knifeComponent.Attack(direction);
//        }
//    }

//    private void Reload()
//    {
//        if (gunComponent != null)
//        {
//            gunComponent.Reload();
//        }
//    }

//    private void GetComponentOfActiveWeapon()
//    {
//        gunComponent = activeWeapon.GetComponent<Gun>();
//        knifeComponent = activeWeapon.GetComponent<Knife>();
//    }
//}
