using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> weaponSlots;
    private int currentWeaponSlot = 0;
    private int size = 4;
    [SerializeField] private int firstAidKitHealth = 50;
    [SerializeField] private float pickRadius = 1f;
    [SerializeField] private GameObject knife;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private FirstAidUI firstAidUI;
    private int firstAidKits = 0;
    private Health health;
    public bool Full {
        get => weaponSlots.Count >= size;
    }
    private PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlots = new List<GameObject>();
        health = GetComponent<Health>();
        playerShooting = GetComponent<PlayerShooting>();
        weaponSlots.Add(knife);
        SetActiveWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UseFirstAid();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForCollectibles();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropActiveWeapon();
        }
    }

    public void AddWeaponToInventory(GameObject weapon)
    {
        if(!Full)
        {
            weaponSlots.Add(weapon);
            ChangeWeapon(weaponSlots.Count - 1);
        }
    }

    public void Remove(GameObject weapon)
    {
        if (Equals(weapon, GetComponent<PlayerShooting>().activeWeapon))
        {
            weaponSlots.Remove(weapon);
        }
    }

    private void ChangeWeapon(int slot)
    {
        if (slot < weaponSlots.Count)
        {
            weaponSlots[currentWeaponSlot].SetActive(false);
            weaponSlots[currentWeaponSlot].transform.SetParent(null);
            SetActiveWeapon(slot);
        }
    }

    private void SetActiveWeapon(int slot)
    {
        if (slot < weaponSlots.Count)
        {
            currentWeaponSlot = slot;
            if (inventoryUI != null && slot > 0)
                inventoryUI.ChooseSlot(slot - 1);
            playerShooting.SetActiveWeapon(weaponSlots[slot]);
        }
        
    }

    private void CheckForCollectibles()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Collectible");
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickRadius, layerMask);
        foreach(var collider in colliders)
        {
            CollectFirstAid(collider);
            CollectAmmunition(collider);
            CollectWeapon(collider);
        }
    }

    private void CollectFirstAid(Collider collider)
    {
        FirstAidKit firstAidKit = collider.GetComponent<FirstAidKit>();
        if (firstAidKit != null)
        {
            firstAidKit.Pick();
            firstAidKits++;
            if(firstAidUI != null)
                firstAidUI.SetNumbers(firstAidKits);
        }
    }

    private void CollectAmmunition(Collider collider)
    {
        Ammunition magazine = collider.GetComponent<Ammunition>();
        if (magazine != null)
        {
            int collectedAmmunition = magazine.Pick();
            DistributeAmmunitionToGuns(collectedAmmunition);
        }
    }

    private void DistributeAmmunitionToGuns(int ammunition)
    {
        int numberOfGuns = weaponSlots.Count - 1;
        if (numberOfGuns == 0)
            return;
        int ammoPerGun = ammunition / numberOfGuns;
        int rest = ammunition % numberOfGuns;
        for(int i = 1; i < weaponSlots.Count; i++)
        {
            int ammo = ammoPerGun + ((i - 1 < rest) ? 1 : 0);
            Gun gun = weaponSlots[i].GetComponent<Gun>();
            gun.AddAmmunition(ammo);
            if (inventoryUI != null)
                inventoryUI.SetAmmo(i - 1, gun.ammunition);
        }
        
    }

    private void CollectWeapon(Collider collider)
    {
        Pick pick = collider.GetComponent<Pick>();
        if(pick != null && !pick.equipped && !Full)
        {
            GameObject weapon = collider.gameObject;
            pick.PickUp(gameObject);
            AddWeaponToInventory(weapon);
            if (inventoryUI != null)
                inventoryUI.AddItemToSlot(currentWeaponSlot - 1, weapon.GetComponent<Gun>().GunSprite, weapon.GetComponent<Gun>().ammunition);
        }
    }

    private void DropActiveWeapon()
    {
        if (currentWeaponSlot != 0)
        {
            GameObject weaponToDrop = weaponSlots[currentWeaponSlot];
            weaponSlots.RemoveAt(currentWeaponSlot);
            int newWeaponSlot = FindActiveWeaponSlotAfterRemove();
            SetActiveWeapon(newWeaponSlot);
            Pick pick = weaponToDrop.GetComponent<Pick>();
            pick.Drop();
            if (inventoryUI != null)
                inventoryUI.DeleteItemFromSlot(currentWeaponSlot);
            for (int i = weaponSlots.Count - 1; i >= currentWeaponSlot && currentWeaponSlot > 0; i-=2)
            {
                inventoryUI.AddItemToSlot(i-1, weaponSlots[i].GetComponent<Gun>().GunSprite, weaponSlots[i].GetComponent<Gun>().ammunition);
                inventoryUI.DeleteItemFromSlot(i);
            }
        }
    }

    private int FindActiveWeaponSlotAfterRemove()
    {
        if (currentWeaponSlot >= weaponSlots.Count)
        {
            return weaponSlots.Count - 1;
        }
        else
        {
            return currentWeaponSlot;
        }
    }

    private void UseFirstAid()
    {
        if(firstAidKits > 0)
        {
            health.Restore(firstAidKitHealth);
            firstAidKits--;
            if (firstAidUI != null)
                firstAidUI.SetNumbers(firstAidKits);
        }
    }
}
