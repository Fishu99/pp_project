using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class generating a gun for a shooting enemy.
/// </summary>
public class EnemyRandomWeapon : MonoBehaviour{

    /// <summary>
    /// List of weapons available for the enemy.
    /// </summary>
    [SerializeField]
    List<GameObject> listOfenemyWeapons = new List<GameObject>();

    /// <summary>
    /// The selected gun.
    /// </summary>
    Gun gun;

    /// <summary>
    /// Returns a gun for the enemy.
    /// The type of the gun is selected randomly from all available weapons, but the more game progress grows, the more powerful weapons are selected.
    /// </summary>
    /// <returns>the selected gun</returns>
    public Gun GetGun() {

        listOfenemyWeapons.Sort((GameObject obj1, GameObject obj2) => {
            Gun gun1 = obj1.GetComponent<Gun>();
            Gun gun2 = obj2.GetComponent<Gun>();

            if (gun1 == null || gun2 == null) {
                return 0;
            }

            return gun1.GetDifficultyAmmunition() > gun2.GetDifficultyAmmunition() ? -1 : 1;
        });

        int max = (int)((listOfenemyWeapons.Count + 1) * (LevelsController.Instance.GetProgress() + Random.Range(0f,0.3f)));
        int min = max - (Random.Range(0,listOfenemyWeapons.Count/2));

        max = Mathf.Clamp(max, 0, listOfenemyWeapons.Count);
        min = Mathf.Clamp(min, 0, max);

        if (min < max) {
            Instantiate(listOfenemyWeapons[Random.Range(min, max)], transform);
        } else {
            Instantiate(listOfenemyWeapons[Mathf.Clamp(max, 0, listOfenemyWeapons.Count - 1)], transform);
        }
        gun = GetComponentInChildren<Gun>();
        gun.AddIgnoreTag("Enemy");
        gun.unlimitedShots = true;
        return gun;
    }

}
