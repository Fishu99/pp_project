using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI for displaying player's inventory.
/// </summary>
public class InventoryUI : MonoBehaviour{

    /// <summary>
    /// A class representing one weapon slot.
    /// </summary>
    [System.Serializable]
    public class Slot{
        /// <summary>
        /// Slot background image.
        /// </summary>
        public Image slot;

        /// <summary>
        /// Slot item (weapon) image.
        /// </summary>
        public Image item;

        /// <summary>
        /// Text displaying the amount of amunition of the weapon.
        /// </summary>
        public TextMeshProUGUI ammo;

        /// <summary>
        /// Current value of ammunition.
        /// </summary>
        public int currentAmmo = 0;

        /// <summary>
        /// Current max value of ammunition.
        /// </summary>
        public int currentMaxAmmo = 0;
    }

    [SerializeField]
    Color notSelectedColor; 

    [SerializeField]
    Color selectedColor; 

    [SerializeField]
    List<Slot> slots = new List<Slot>();

    [Header("Sounds")]

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip changeWeaponSound;

    [SerializeField]
    AudioClip getWeaponSound;

    [SerializeField]
    AudioClip dropWeaponSound;

    [SerializeField]
    AudioClip getAmmoSound;

    [SerializeField]
    AudioClip reloadSound;

    int currentSelectedWeaponIndex = -1;

    void Awake(){
        foreach(Slot slot in slots){
            slot.item.gameObject.SetActive(false);
            slot.slot.color = notSelectedColor;     
            slot.ammo.text = "";
        }
    }

    /// <summary>
    /// Adds an item to a slot.
    /// </summary>
    /// <param name="index">the index of the slot where the item will be added</param>
    /// <param name="sprite">a sprite of he item which will be displayed in the slot</param>
    public void AddItemToSlot(int index, Sprite sprite){
        if(slots.Count > index && index >= 0){
            slots[index].item.sprite = sprite;
            slots[index].ammo.text = "";
            slots[index].item.gameObject.SetActive(true);
            audioSource.PlayOneShot(getWeaponSound);
        }
    }

    /// <summary>
    /// Adds an item to a slot.
    /// </summary>
    /// <param name="index">the index of the slot where the item will be added</param>
    /// <param name="sprite">a sprite of he item which will be displayed in the slot</param>
    /// <param name="ammo">current value of ammunition</param>
    /// <param name="maxAmmo">current max value of ammunition</param>
    public void AddItemToSlot(int index, Sprite sprite, int ammo, int maxAmmo){
        if(slots.Count > index && index >= 0){
            slots[index].item.sprite = sprite;
            SetAmmoInScript(index, ammo, maxAmmo);
            slots[index].item.gameObject.SetActive(true);
            audioSource.PlayOneShot(getWeaponSound);
        }
    }

    /// <summary>
    /// Deletes an item from a slot.
    /// </summary>
    /// <param name="index">the index of the slot from which the item should be removed</param>
    public void DeleteItemFromSlot(int index){
        if(slots.Count > index && index >= 0){
            slots[index].item.gameObject.SetActive(false);
            slots[index].ammo.text = "";
            audioSource.PlayOneShot(dropWeaponSound);
        }
    }

    /// <summary>
    /// Selects a slot, making it highlighted.
    /// </summary>
    /// <param name="index">the index of the slot to select.</param>
    public void ChooseSlot(int index){

        if(index != currentSelectedWeaponIndex){
            foreach(Slot slot in slots){
                slot.slot.color = notSelectedColor;
            }
            if(slots.Count > index && index >= 0){
                slots[index].slot.color = selectedColor;
            }
            currentSelectedWeaponIndex = index;
            audioSource.PlayOneShot(changeWeaponSound);
        }
    }

    /// <summary>
    /// Sets the ammunition of an item in a slot.
    /// </summary>
    /// <param name="index">the index of the slot</param>
    /// <param name="ammo">current value of ammunition</param>
    /// <param name="maxAmmo">current max value of ammunition</param>
    public void SetAmmo(int index, int ammo, int maxAmmo){
        if(slots.Count > index && index >= 0){
            if(maxAmmo > slots[index].currentMaxAmmo){
                audioSource.PlayOneShot(getAmmoSound);
            }else if(ammo > slots[index].currentAmmo){
                audioSource.PlayOneShot(reloadSound);
            }
            SetAmmoInScript(index, ammo, maxAmmo);
        }
    }

    void SetAmmoInScript(int index, int ammo, int maxAmmo){
        slots[index].ammo.text = ammo.ToString() + "/" + maxAmmo;
        slots[index].currentAmmo = ammo;
        slots[index].currentMaxAmmo = maxAmmo;
    }
    
}
