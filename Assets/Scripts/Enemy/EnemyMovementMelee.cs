using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementMelee : EnemyMovement
{
    [SerializeField] protected float playerCloseToAttackRange;

    protected override void SetStateDetectedValues() {
        
    }

    protected override void UnsetStateDetectedValues() {
        SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
        if(navMeshAgent) navMeshAgent.isStopped = false;
    }

    protected override void Detected() {
        Vector3 dir = (player.transform.position - rigidbody.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, q, Time.fixedDeltaTime * navMeshAgent.angularSpeed * 0.5f);

        if((player.transform.position-transform.position).magnitude > playerCloseToAttackRange) {
            if(navMeshAgent) navMeshAgent.isStopped = false;

            GetComponent<Animator>().SetBool("isWalking", true);

            navMeshAgent?.SetDestination(player.transform.position);
        }
        else {
            if(navMeshAgent) navMeshAgent.isStopped = true;

            if(timerManager.GetStatusOfTimer("ACD") <= 0) {
                enemyMelee?.Attack(transform.position);
                timerManager.ResetTimer("ACD");
            }
        }

        if((player.transform.position-transform.position).magnitude > playerDetectRange)
            SetState(EnemyStates.UNDETECTED);
    }

    protected virtual void OnDrawGizmos() {
        base.OnDrawGizmos();
        if(debugGizmos)
		{
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerCloseToAttackRange);
        }
    }
}
