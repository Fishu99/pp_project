using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour{

    [System.Serializable]
    public class Slot{
        public Image slot;
        public Image item;
        public TextMeshProUGUI ammo;
        public int currentAmmo = 0;
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

    public void AddItemToSlot(int index, Sprite sprite){
        if(slots.Count > index && index >= 0){
            slots[index].item.sprite = sprite;
            slots[index].ammo.text = "";
            slots[index].item.gameObject.SetActive(true);
            audioSource.PlayOneShot(getWeaponSound);
        }
    }

    public void AddItemToSlot(int index, Sprite sprite, int ammo, int maxAmmo){
        if(slots.Count > index && index >= 0){
            slots[index].item.sprite = sprite;
            SetAmmoInScript(index, ammo, maxAmmo);
            slots[index].item.gameObject.SetActive(true);
            audioSource.PlayOneShot(getWeaponSound);
        }
    }

    public void DeleteItemFromSlot(int index){
        if(slots.Count > index && index >= 0){
            slots[index].item.gameObject.SetActive(false);
            slots[index].ammo.text = "";
            audioSource.PlayOneShot(dropWeaponSound);
        }
    }

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
