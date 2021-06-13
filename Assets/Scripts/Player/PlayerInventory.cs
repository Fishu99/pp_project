using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> weaponSlots;
    private int size = 3;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlots = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponSlots.Count >= 1)
        {
           // GetComponent<PlayerShooting>().activeWeapon.SetActive(false);
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[0]);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && weaponSlots.Count >= 2)
        {
            //GetComponent<PlayerShooting>().activeWeapon.SetActive(false);
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[1]);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && weaponSlots.Count >= 3)
        {
           //GetComponent<PlayerShooting>().activeWeapon.SetActive(false);
            GetComponent<PlayerShooting>().SetActiveWeapon(weaponSlots[2]);
        }
    }

    public void Add(GameObject weapon)
    {
        if(weaponSlots.Count < size)
        {
            //sGetComponent<PlayerShooting>().activeWeapon.SetActive(false);
            GetComponent<PlayerShooting>().SetActiveWeapon(weapon);
            weaponSlots.Add(weapon);
        }
    }

    public void Remove(GameObject weapon)
    {
        if (Equals(weapon, GetComponent<PlayerShooting>().activeWeapon))
        {
            weaponSlots.Remove(weapon);
        }
    }
}
