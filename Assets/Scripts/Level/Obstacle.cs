using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which defines information about an obstacle in the room.
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// Type of the obstacle.
    /// </summary>
    [SerializeField] private int obstacleType;

    /// <summary>
    /// Access to obstacleType variable.
    /// </summary>
    /// <returns>obstacleType variable</returns>
    public int getType() {
        return obstacleType;
    }
}
