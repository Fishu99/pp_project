using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("SpawnPoint"))
           Destroy(other.transform.gameObject);
    }
}
