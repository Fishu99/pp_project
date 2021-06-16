using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> weaponSlots;
    private int size = 3;
    [SerializeField] private int firstAidKitHealth = 50;
    [SerializeField] private float pickRadius = 1f;
    private int firstAidKits = 0;
    private Health health;
    public bool full = false;
    public int ammunition = 0;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlots = new List<GameObject>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponSlots.Count >= 1)
        {
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[0]);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && weaponSlots.Count >= 2)
        {
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[1]);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && weaponSlots.Count >= 3)
        {
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[2]);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UseFirstAid();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForCollectibles();
        }
    }

    public void Add(GameObject weapon)
    {
        if(weaponSlots.Count < size)
        {
            GetComponent<PlayerShooting>().SetActiveWeapon(weapon);
            weaponSlots.Add(weapon);
            if (weaponSlots.Count == 3)
            {
                full = true;
            }
        }
    }

    public void Remove(GameObject weapon)
    {
        if (Equals(weapon, GetComponent<PlayerShooting>().activeWeapon))
        {
            weaponSlots.Remove(weapon);
        }
        if (weaponSlots.Count < size)
        {
            full = false;
        }
    }

    private void CheckForCollectibles()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Collectible");
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickRadius, layerMask);
        foreach(var collider in colliders)
        {
            FirstAidKit firstAidKit = collider.GetComponent<FirstAidKit>();
            if (firstAidKit != null)
            {
                firstAidKit.Pick();
                firstAidKits++;
            }
            Ammunition magazine = collider.GetComponent<Ammunition>();
            if (magazine != null)
            {
                int collectedAmmunition = magazine.Pick();
                ammunition += collectedAmmunition;
            }
        }
    }
    private void UseFirstAid()
    {
        if(firstAidKits > 0)
        {
            health.Restore(firstAidKitHealth);
            firstAidKits--;
        }
    }
}
