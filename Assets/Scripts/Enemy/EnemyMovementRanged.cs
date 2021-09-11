using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class controlling the ranged enemy's movement.
/// </summary>
public class EnemyMovementRanged : EnemyMovement
{
    /// <summary>
    /// A point from which the enemy attacks.
    /// </summary>
    [SerializeField] Transform shootingPosition;

    /// <summary>
    /// Method called when the enemy enters the DETECTED state.
    /// </summary>
    protected override void SetStateDetectedValues() {
        if(navMeshAgent) navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// Method called when the enemy exits the DETECTED state.
    /// </summary>
    protected override void UnsetStateDetectedValues() {
        if(navMeshAgent) navMeshAgent.isStopped = false;
    }

    /// <summary>
    /// Enemy's behavior in the DETECTED state. The method is called in every Update if the enemy's state is DETECTED.
    /// The enemy rotates towards the player and periodically shoots at the player.
    /// </summary>
    protected override void Detected() {
        Vector3 dir = (player.transform.position - rigidbody.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, q, Time.fixedDeltaTime * navMeshAgent.angularSpeed * 0.5f);
           
        if(timerManager.GetStatusOfTimer("ACD") <= 0) {
            enemyShooting?.Attack(shootingPosition == null ? transform.position : shootingPosition.position);
            timerManager.ResetTimer("ACD");
        }

        if (!CanSeePlayer())
            SetState(EnemyStates.SEARCHING);
    }
}
