using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for weapons which can be picked and dropped by the player.
/// </summary>
public class Pick : MonoBehaviour
{
    /// <summary>
    /// Gun component of the object.
    /// </summary>
    [SerializeField] private Gun gunScript;
    [SerializeField] private GameObject weapon;

    /// <summary>
    /// The rigidbody component of the weapon.
    /// </summary>
    [SerializeField] private Rigidbody weaponRigidbody;

    /// <summary>
    /// The collider of the weapon.
    /// </summary>
    [SerializeField] private BoxCollider weaponCollider;
    
    /// <summary>
    /// The player character.
    /// </summary>
    private GameObject player;

    [SerializeField] private float dropForwardForce, dropUpwardForce;

    /// <summary>
    /// Gets or sets the value indicating if the weapon is possessed by the player.
    /// </summary>
    public bool equipped { get; private set; }

    /// <summary>
    /// Initialization of the component.
    /// </summary>
    private void Start()
    {
        //Setup
        if (!equipped)
        {
            gunScript.enabled = false;
            weaponRigidbody.isKinematic = false;
            weaponCollider.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            weaponRigidbody.isKinematic = true;
            weaponCollider.isTrigger = true;
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// Picks up the weapon.
    /// </summary>
    /// <param name="player">The player which picks up the weapon.</param>
    public void PickUp(GameObject player)
    {

        this.player = player;
        equipped = true;

        //Make weaponRigidbody kinematic and BoxCollider a trigger
        weaponRigidbody.isKinematic = true;
        weaponCollider.isTrigger = true;


        //Enable script
        gunScript.enabled = true;
    }

    /// <summary>
    /// Drops the weapon.
    /// </summary>
    public void Drop()
    {
        equipped = false;

        //Set parent to null
        transform.SetParent(null);

        //Make weaponRigidbody not kinematic and BoxCollider normal
        weaponRigidbody.isKinematic = false;
        weaponCollider.isTrigger = false;

        //Gun carries momentum of player
        weaponRigidbody.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        weaponRigidbody.AddForce(player.transform.forward * dropForwardForce, ForceMode.Impulse);
        weaponRigidbody.AddForce(player.transform.up * dropUpwardForce, ForceMode.Impulse);

        //Add random rotation
        float random = Random.Range(-1f, 1f);
        weaponRigidbody.AddTorque(new Vector3(random, random, random) * 10);

        //remove

        //Disable script
        gunScript.enabled = false;
    }
}
