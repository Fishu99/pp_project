using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    [SerializeField] private Gun gunScript;
    [SerializeField] private GameObject weapon;
    [SerializeField] private Rigidbody weaponRigidbody;
    [SerializeField] private BoxCollider weaponCollider;
    [SerializeField] private Transform gunContainer, fpsCam;
    [SerializeField] private GameObject player;

    [SerializeField] private float pickUpRange;
    [SerializeField] private float dropForwardForce, dropUpwardForce;

    [SerializeField] private bool equipped;

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
        //Check if player is in range and "E" is pressed
        Vector3 distanceToPlayer = player.transform.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E)) PickUp();

        //Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    private void PickUp()
    {
        if (!player.GetComponent<PlayerInventory>().full)
        {
            equipped = true;

            //Make weapon a child of the camera and move it to default position
            //transform.SetParent(gunContainer);
            transform.localPosition = Vector3.up;
            transform.localRotation = Quaternion.Euler(0, 180, 0);

            //Make weaponRigidbody kinematic and BoxCollider a trigger
            weaponRigidbody.isKinematic = true;
            weaponCollider.isTrigger = true;

            player.GetComponent<PlayerInventory>().Add(weapon);
            player.GetComponent<PlayerShooting>().SetActiveWeapon(weapon);

            //Enable script
            gunScript.enabled = true;
        }
    }

    private void Drop()
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
        weaponRigidbody.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        weaponRigidbody.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        weaponRigidbody.AddTorque(new Vector3(random, random, random) * 10);

        //remove
        player.GetComponent<PlayerInventory>().Remove(weapon);
        player.GetComponent<PlayerShooting>().activeWeapon = null;

        //Disable script
        gunScript.enabled = false;
    }
}
