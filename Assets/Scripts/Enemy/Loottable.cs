using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loottable : MonoBehaviour
{
    [SerializeField] List<GameObject> lootObjects = new List<GameObject>();
    [SerializeField] List<int> lootChances = new List<int>();
    [SerializeField] float maxLootSpreadRadius;

    bool wasDrop = false;

    private void Start()
    {
        wasDrop = false;
    }

    public void DropItems() {

        if (wasDrop)
            return;

        float chance;
        Vector3 spreadVector = Vector3.zero;
        wasDrop = true;
        for(int i = 0; i < lootObjects.Count && i < lootChances.Count; i++) {

            if (lootChances[i] == null && lootObjects[i] == null)
                continue;

            chance = Random.Range(1, 101);

            if(chance <= lootChances[i]) {
                spreadVector.x = (float)(Random.Range(0, maxLootSpreadRadius * 100))/100f;
                spreadVector.z = (float)(Random.Range(0, maxLootSpreadRadius * 100))/100f;

                GameObject loot = Instantiate(lootObjects[i], gameObject.transform.position + spreadVector, Quaternion.identity) as GameObject;
                loot.name = lootObjects[i].name;
            }

        }
    }
}
