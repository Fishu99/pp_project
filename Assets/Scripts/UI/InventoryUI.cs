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
    }

    [SerializeField]
    Color notSelectedColor; 

    [SerializeField]
    Color selectedColor; 

    [SerializeField]
    List<Slot> slots = new List<Slot>();

    void Awake(){
        foreach(Slot slot in slots){
            slot.item.gameObject.SetActive(false);
            slot.slot.color = notSelectedColor;     
            slot.ammo.text = "";
        }
    }

    public void AddItemToSlot(int index, Sprite sprite){
        if(slots.Count > index){
            slots[index].item.sprite = sprite;
            slots[index].ammo.text = "";
            slots[index].item.gameObject.SetActive(true);
        }
    }

    public void AddItemToSlot(int index, Sprite sprite, int ammo){
        if(slots.Count > index){
            slots[index].item.sprite = sprite;
            slots[index].ammo.text = ammo.ToString();
            slots[index].item.gameObject.SetActive(true);
        }
    }

    public void DeleteItemFromSlot(int index){
        if(slots.Count > index){
            slots[index].item.gameObject.SetActive(false);
            slots[index].ammo.text = "";
        }
    }

    public void ChooseSlot(int index){
        if(slots.Count > index){
            foreach(Slot slot in slots){
                slot.slot.color = notSelectedColor;
            }
            slots[index].slot.color = selectedColor;
        }
    }

    public void SetAmmo(int index, int ammo){
        if(slots.Count > index){
            slots[index].ammo.text = ammo.ToString();
        }
    }
    
}
