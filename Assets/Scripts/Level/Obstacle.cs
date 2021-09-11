using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for an obstacle placed in a room.
/// </summary>
public class Obstacle : MonoBehaviour
{
    /// <summary>
    /// A number specifying the obstacle type.
    /// </summary>
    [SerializeField] private int obstacleType;

    /// <summary>
    /// Returns a number specifying the obstacle type.
    /// </summary>
    /// <returns>a number specifying the obstacle type</returns>
    public int getType() {
        return obstacleType;
    }
}
