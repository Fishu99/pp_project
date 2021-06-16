using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoints : MonoBehaviour
{
    private List<EnemyWaypoint> waypoints;

    void Awake()
    {
        LoadWaypoints();
    }

    public EnemyWaypoint GetWaypoint(int index) {
        Debug.Log(waypoints);
        return waypoints[index];
    }

    public bool IsWaypointInReach(int index, Transform transform, float distance) {
        if((waypoints[index].transform.position - transform.position).magnitude < distance) return true;
        else return false;
    }

    private void LoadWaypoints()
    {
        EnemyWaypoint[] enemyWaypointTable = GetComponentsInChildren<EnemyWaypoint>();
        waypoints = new List<EnemyWaypoint>(enemyWaypointTable);
    }

    public int GetWaypointCount() {
        return waypoints.Count;
    }
}
