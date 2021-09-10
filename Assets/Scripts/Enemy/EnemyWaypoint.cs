using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for enemy's waypoint.
/// </summary>
public class EnemyWaypoint : MonoBehaviour
{
    /// <summary>
    /// When set to true, the gizmos will be shown in debug mode.
    /// </summary>
    [SerializeField] private bool debugGizmos;

    /// <summary>
    /// Draws a blue wire sphere gizmo with a center in the waypoint's position.
    /// </summary>
    void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
