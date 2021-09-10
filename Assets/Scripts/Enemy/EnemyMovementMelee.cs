using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class controlling the meleee enemy's movement.
/// </summary>
public class EnemyMovementMelee : EnemyMovement
{
    /// <summary>
    /// The distance from the player when the enemy starts to attack.
    /// </summary>
    [SerializeField] protected float playerCloseToAttackRange;

    /// <summary>
    /// Method called when the enemy enters the DETECTED state.
    /// </summary>
    protected override void SetStateDetectedValues() {

    }

    /// <summary>
    /// Method called when the enemy exits the DETECTED state.
    /// </summary>
    protected override void UnsetStateDetectedValues() {
        SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
        if(navMeshAgent) navMeshAgent.isStopped = false;
    }

    /// <summary>
    /// Enemy's behavior in the DETECTED state. The method is called in every Update if the enemy's state is DETECTED.
    /// The enemy follows the player and attacks if the distance to the player is lower than playerCloseToAttackRange.
    /// </summary>
    protected override void Detected() {
        Vector3 dir = (player.transform.position - rigidbody.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, q, Time.fixedDeltaTime * navMeshAgent.angularSpeed * 0.5f);

        if((player.transform.position-transform.position).magnitude > playerCloseToAttackRange) {
            if(navMeshAgent != null && navMeshAgent.isActiveAndEnabled) navMeshAgent.isStopped = false;

            GetComponent<Animator>().SetBool("isWalking", true);

            if(navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                navMeshAgent.SetDestination(player.transform.position);
        }
        else {
            if(navMeshAgent != null && navMeshAgent.isActiveAndEnabled) navMeshAgent.isStopped = true;

            if(timerManager.GetStatusOfTimer("ACD") <= 0) {
                enemyMelee?.Attack(transform.position);
                timerManager.ResetTimer("ACD");
            }
        }

        if(!CanSeePlayer())
            SetState(EnemyStates.SEARCHING);
    }

    /// <summary>
    /// Draws a yellow wire sphere gizmo with a radius equal to playerCloseToAttackRange.
    /// </summary>
    protected virtual void OnDrawGizmos() {
        base.OnDrawGizmos();
        if(debugGizmos)
		{
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerCloseToAttackRange);
        }
    }
}
