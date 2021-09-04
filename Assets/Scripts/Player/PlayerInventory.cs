using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for controlling the player's inventory.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// List of slots for collected weapons.
    /// </summary>
    private List<GameObject> weaponSlots;
    /// <summary>
    /// Currently selected weapon slot.
    /// </summary>
    private int currentWeaponSlot = 0;
    /// <summary>
    /// Max size of the weapon inventory.
    /// </summary>
    private int size = 4;
    /// <summary>
    /// The health restored by one first aid kit.
    /// </summary>
    [SerializeField] private int firstAidKitHealth = 50;
    /// <summary>
    /// The radius in which objects lying on the floor can be picked into the inventory.
    /// </summary>
    [SerializeField] private float pickRadius = 1f;
    /// <summary>
    /// The knife possessed by the player.
    /// </summary>
    [SerializeField] private GameObject knife;
    /// <summary>
    /// The UI displaying the status of the inventory.
    /// </summary>
    [SerializeField] private InventoryUI inventoryUI;
    /// <summary>
    /// The UI displaying the number of first aid kits.
    /// </summary>
    [SerializeField] private FirstAidUI firstAidUI;
    /// <summary>
    /// The UI displaying the type of en item which can be picked.
    /// </summary>
    [SerializeField] private ItemInfo itemInfo;
    /// <summary>
    /// The nearest collectible item.
    /// </summary>
    Collider nearestCollectible = null;
    /// <summary>
    /// Number of first aid kits possessed by the player.
    /// </summary>
    private int firstAidKits = 0;
    /// <summary>
    /// The Health component of the player.
    /// </summary>
    private Health health;
    /// <summary>
    /// Returns true if the player has a maximal number of weapons.
    /// </summary>
    public bool Full {
        get => weaponSlots.Count >= size;
    }
    /// <summary>
    /// A component controlling the player's shooting
    /// </summary>
    private PlayerShooting playerShooting;

    /// <summary>
    /// Initializes the weapon slots and gets the component references.
    /// </summary>
    void Start()
    {
        weaponSlots = new List<GameObject>();
        health = GetComponent<Health>();
        playerShooting = GetComponent<PlayerShooting>();
        weaponSlots.Add(knife);
        SetActiveWeapon(0);
    }

    /// <summary>
    /// Handles user input.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
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
       
        ViewOfCollectibles();
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForCollectibles();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropActiveWeapon();
        }

        if (inventoryUI != null)
        {
            for (int i = 0; i < weaponSlots.Count; i++)
            {
                Gun gun = weaponSlots[i].GetComponent<Gun>();
                if (weaponSlots[i] != null && gun != null)
                {
                    inventoryUI.SetAmmo(i - 1, AmmoInMagazine(gun) , AmmoInPocket(gun));
                }
            }
        }
    }

    /// <summary>
    /// Adds a weapon to the inventory if the inventory is not full. The weapon is placed in the lowest available slot.
    /// </summary>
    /// <param name="weapon">The weapon to add</param>
    public void AddWeaponToInventory(GameObject weapon)
    {
        if(!Full)
        {
            weaponSlots.Add(weapon);
            ChangeWeapon(weaponSlots.Count - 1);
        }
    }

    /// <summary>
    /// Removes a weapon from the inventory.
    /// </summary>
    /// <param name="weapon">The weapon to remove.</param>
    public void Remove(GameObject weapon)
    {
        if (Equals(weapon, GetComponent<PlayerShooting>().activeWeapon))
        {
            weaponSlots.Remove(weapon);
        }
    }

    /// <summary>
    /// Deactivates the current active weapon and sets a new weapon as the active one.
    /// </summary>
    /// <param name="slot">The number of slot in which the weapon to activate is placed.</param>
    private void ChangeWeapon(int slot)
    {
        if (slot < weaponSlots.Count)
        {
            weaponSlots[currentWeaponSlot].SetActive(false);
            weaponSlots[currentWeaponSlot].transform.SetParent(null);
            SetActiveWeapon(slot);
        }
    }

    /// <summary>
    /// Sets a weapon as the active one. The active weapon is the weapon which is carried and shot by the player.
    /// </summary>
    /// <param name="slot">The number of slot in which the weapon to activate is placed.</param>
    private void SetActiveWeapon(int slot)
    {
        if (slot < weaponSlots.Count)
        {
            currentWeaponSlot = slot;
            if (inventoryUI != null && slot >= 0)
                inventoryUI.ChooseSlot(slot - 1);
            
            weaponSlots[slot].GetComponent<Gun>()?.AddIgnoreTag("Player");
            playerShooting.SetActiveWeapon(weaponSlots[slot]);
        }
        
    }

    /// <summary>
    /// Checks if there are collectibles within the pickRadius from the player.
    /// If there are any, the nearest of them is set as the nearestCollectible.
    /// The itemInfo about the nearestCollectible is displayed too.
    /// </summary>
    private void ViewOfCollectibles(){

        int layerMask = 1 << LayerMask.NameToLayer("Collectible");
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickRadius, layerMask);
        nearestCollectible = null;
        float distance = float.MaxValue;
        foreach(var collider in colliders)
        {
            if(nearestCollectible == null || Vector3.Distance(nearestCollectible.transform.position,collider.transform.position) < distance)
            {
                distance = Vector3.Distance(collider.transform.position,transform.position);
                nearestCollectible = collider;
            }
        }

        if(itemInfo != null)
        {
            if(nearestCollectible == null)
            {
                itemInfo.Hide();
            }
            else
            {
                itemInfo.Show(nearestCollectible.gameObject.name, nearestCollectible.transform.position);     
            }
        }
        

    }

    /// <summary>
    /// Collects the nearest collectible.
    /// </summary>
    private void CheckForCollectibles()
    {  
        if(nearestCollectible != null)
        {
            CollectFirstAid(nearestCollectible);
            CollectAmmunition(nearestCollectible);
            CollectWeapon(nearestCollectible);
        }
        
    }

    /// <summary>
    /// Collects the first aid kit if the collider's GameObject has the FirstAidKit component.
    /// </summary>
    /// <param name="collider">the collider of the object to collect.</param>
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

    /// <summary>
    /// Collects the ammunition if the collider's GameObject has the Ammunition component.
    /// </summary>
    /// <param name="collider">the collider of the object to collect.</param>
    private void CollectAmmunition(Collider collider)
    {
        Ammunition magazine = collider.GetComponent<Ammunition>();
        if (magazine != null)
        {
            int collectedAmmunition = magazine.Pick();
            DistributeAmmunitionToGuns(collectedAmmunition);
        }
    }

    /// <summary>
    /// Distributes the collected ammunition to the guns in the inventory.
    /// The ammunition is distributed evenly to all guns.
    /// If the number of ammunition is not divisible by the number of guns,
    /// the guns from the lowest slots gat one more piece of ammunition then the other ones.
    /// </summary>
    /// <param name="ammunition"></param>
    private void DistributeAmmunitionToGuns(int ammunition)
    {
        int numberOfGuns = weaponSlots.Count - 1;
        if (numberOfGuns == 0)
            return;
        int ammoPerGun = ammunition / numberOfGuns;
        int rest = ammunition % numberOfGuns;
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            int ammo = ammoPerGun + ((i - 1 < rest) ? 1 : 0);
            Gun gun = weaponSlots[i].GetComponent<Gun>();
            if (weaponSlots[i] != null && gun != null)
            {
                gun.AddAmmunition(ammo);
            }
        }
        if (inventoryUI != null)
        {
            for(int i = 0; i < weaponSlots.Count; i++)
            {
                Gun gun = weaponSlots[i].GetComponent<Gun>();
                if (weaponSlots[i] != null && gun != null)
                {
                    inventoryUI.SetAmmo(i - 1, AmmoInMagazine(gun), AmmoInPocket(gun));
                }
            }
        }
    }

    /// <summary>
    /// Collects the gun if the collider's GameObject has the Gun component.
    /// </summary>
    /// <param name="collider">the collider of the object to collect.</param>
    private void CollectWeapon(Collider collider)
    {
        Pick pick = collider.GetComponent<Pick>();
        if(pick != null && !pick.equipped && !Full)
        {
            GameObject weapon = collider.gameObject;
            pick.PickUp(gameObject);
            AddWeaponToInventory(weapon);
            Gun gun = weapon.GetComponent<Gun>();
            if (inventoryUI != null){
                inventoryUI.AddItemToSlot(currentWeaponSlot - 1, gun.GunSprite, AmmoInMagazine(gun) , AmmoInPocket(gun));
            }
        }
    }

    /// <summary>
    /// Drops the active weapon. The dropped weapon is removed from the inventory and put on the floor in front of the player.
    /// </summary>
    private void DropActiveWeapon()
    {
        if (currentWeaponSlot != 0)
        {
            GameObject weaponToDrop = weaponSlots[currentWeaponSlot];
            weaponSlots.RemoveAt(currentWeaponSlot);
            int newWeaponSlot = FindActiveWeaponSlotAfterRemove();
            SetActiveWeapon(newWeaponSlot);
            Pick pick = weaponToDrop.GetComponent<Pick>();
            weaponToDrop.GetComponent<Gun>()?.DeleteIgnoreTag("Player");
            pick.Drop();
            if (inventoryUI != null)
                inventoryUI.DeleteItemFromSlot(currentWeaponSlot);
            if (currentWeaponSlot > 0)
            {
                for (int i = currentWeaponSlot - 1; i < weaponSlots.Count - 1; i++)
                {
                    Gun gun = weaponSlots[i + 1].GetComponent<Gun>();
                    inventoryUI.AddItemToSlot(i, gun.GunSprite, AmmoInMagazine(gun) , AmmoInPocket(gun));
                    inventoryUI.DeleteItemFromSlot(i + 1);
                }
            }
        }
    }

    /// <summary>
    /// Finds the slot of weapon which will be active after the current active weapon is removed.
    /// </summary>
    /// <returns>the slot of weapon which will be active after the current active weapon is removed</returns>
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

    /// <summary>
    /// If the player has any first aid kit, the method restores player's health and decrements the number of first aid kits in the inventory.
    /// </summary>
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

    /// <summary>
    /// Returns the number of ammunition in the magazine of the gun, i.e. the number of ammunition which can be shot before reload.
    /// </summary>
    /// <param name="gun">The gun to check.</param>
    /// <returns>number of ammunition in the magazine of the gun</returns>
    private int AmmoInMagazine(Gun gun){
        return Mathf.Clamp(gun.reloadCounter, 0,  gun.GetAmmunition());
    }

    /// <summary>
    /// Returns the number of ammunition in the pocket of the gun, i.e. the number of ammunition which can be used for reloading the gun.
    /// </summary>
    /// <param name="gun">The gun to check.</param>
    /// <returns>number of ammunition in the pocket of the gun</returns>
    private int AmmoInPocket(Gun gun){
        return Mathf.Clamp((gun.GetAmmunition() - gun.reloadCounter), 0 , int.MaxValue);
    }
}
