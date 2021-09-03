using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for the camera which makes it follow the player.
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    /// <summary>
    /// Offset from the player to the camera.
    /// </summary>
    [SerializeField] private Vector3 playerOffset;
    /// <summary>
    /// The player followed by the camera.
    /// </summary>
    public GameObject player { get; set; }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Updates the camera's position according to the current position of the followed player.
    /// It should be called after calculating the position of the player to prevent jerky motion.
    /// </summary>
    public void updatePosition()
    {
        if (player != null)
        {
            transform.position = player.transform.position + playerOffset;
        }
    }

}
