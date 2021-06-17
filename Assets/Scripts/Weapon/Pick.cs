using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    [SerializeField] private Gun gunScript;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Rigidbody weaponRigidbody;
    [SerializeField] private BoxCollider weaponCollider;
    //[SerializeField] private Transform weaponGrip;
    private GameObject player;

    //[SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce, dropUpwardForce;

    public bool equipped { get; private set; }

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
