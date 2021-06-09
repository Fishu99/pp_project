using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Weapon weaponComponent;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        weaponComponent = weapon.GetComponent<Weapon>();
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
        weapon.GetComponent<Weapon>().Reload();
    }
}
