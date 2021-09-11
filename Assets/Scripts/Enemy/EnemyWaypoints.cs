using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class managing the enemy waypoints.
/// </summary>
public class EnemyWaypoints : MonoBehaviour
{
    /// <summary>
    /// The list of enemy waypoints.
    /// </summary>
    private List<EnemyWaypoint> waypoints = new List<EnemyWaypoint>();

    /// <summary>
    /// Initializes the list of waypoints.
    /// </summary>
    void Awake(){
        LoadWaypoints();
    }

    /// <summary>
    /// Gets a waypoint.
    /// </summary>
    /// <param name="index">index of the waypoint</param>
    /// <returns>the waypoint of the given index</returns>
    public EnemyWaypoint GetWaypoint(int index) {

        if (waypoints.Count <= index){
            return null;
        }

        return waypoints[index];
    }

    /// <summary>
    /// Tells if a waypoint is within the specified distance from the given point.
    /// </summary>
    /// <param name="index">the index of the waypoint</param>
    /// <param name="transform">the transform of the object to check</param>
    /// <param name="distance">max distance from the point</param>
    /// <returns>true if the waypoint is within the specified distance from the given point</returns>
    public bool IsWaypointInReach(int index, Transform transform, float distance) {

        if(waypoints.Count <= index){
            return true;
        }

        if ((waypoints[index].transform.position - transform.position).magnitude < distance){
            return true;
        }else {
            return false;
        }
    }

    /// <summary>
    /// Loads all the EnemyWaypoint components from the enemy's children.
    /// </summary>
    private void LoadWaypoints()
    {
        EnemyWaypoint[] enemyWaypointTable = GetComponentsInChildren<EnemyWaypoint>();
        waypoints = new List<EnemyWaypoint>(enemyWaypointTable);
    }

    /// <summary>
    /// Returns the number of the waypoints.
    /// </summary>
    /// <returns></returns>
    public int GetWaypointCount() {
        return waypoints.Count;
    }
}
