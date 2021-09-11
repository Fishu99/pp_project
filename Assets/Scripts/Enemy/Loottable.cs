using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for managing items dropped by the enemy
/// </summary>
public class Loottable : MonoBehaviour
{
    /// <summary>
    /// List of the objects which can be dropped.
    /// </summary>
    [SerializeField] List<GameObject> lootObjects = new List<GameObject>();

    /// <summary>
    /// Chances of dropping of each object in %.
    /// </summary>
    [SerializeField] List<int> lootChances = new List<int>();

    /// <summary>
    /// Mas radius in which the items will be spread.
    /// </summary>
    [SerializeField] float maxLootSpreadRadius;

    /// <summary>
    /// Tells if the items were dropped.
    /// </summary>
    bool wasDrop = false;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Start()
    {
        wasDrop = false;
    }

    /// <summary>
    /// Drops the items. Each item from the lootObjects is dropped with a corresponding chance from lootChances.
    /// The items are dropped within a circle described by maxLootSpreadRadius.
    /// </summary>
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
