using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [SerializeField] private Vector3 playerOffset;
    public GameObject player { get; set; }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void updatePosition()
    {
        if (player != null)
        {
            transform.position = player.transform.position + playerOffset;
        }
    }

}
