using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
    [SerializeField] private bool debugGizmos;
    void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
