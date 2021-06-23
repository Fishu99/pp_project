using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loottable : MonoBehaviour
{
    [SerializeField] List<GameObject> lootObjects = new List<GameObject>();
    [SerializeField] List<int> lootChances = new List<int>();
    [SerializeField] float maxLootSpreadRadius;

    public void DropItems() {
        float chance;
        Vector3 spreadVector = Vector3.zero;

        int i = 0;
        foreach(float lootChance in lootChances) {
            chance = Random.Range(1, 101);
            //Debug.Log("Chance:" + chance + "<=" + lootChance);
            if(chance <= lootChance) {
                spreadVector.x = (float)(Random.Range(0, maxLootSpreadRadius * 100))/100f;
                spreadVector.z = (float)(Random.Range(0, maxLootSpreadRadius * 100))/100f;
                //Debug.Log("Spawned! Spread:" + spreadVector);

                GameObject loot = Instantiate(lootObjects[i], gameObject.transform.position + spreadVector, Quaternion.identity) as GameObject;
            }
            i++;
        }
    }
}
