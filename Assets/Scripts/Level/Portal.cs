using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour{

    void OnTriggerEnter(Collider other){
        PlayerMovement movement = other.GetComponent<PlayerMovement>();

        if (movement != null){
            LevelsController.Instance.GoLevelUp();
            movement.transform.position = Vector3.zero;
        }
        
    }

}
