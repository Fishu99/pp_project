using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementRanged : EnemyMovement
{
    protected override void SetStateDetectedValues() {
        if(navMeshAgent) navMeshAgent.isStopped = true;
        GetComponent<Animator>().SetBool("isWalking", false);
    }
    
    protected override void UnsetStateDetectedValues() {
        if(navMeshAgent) navMeshAgent.isStopped = false;
        GetComponent<Animator>().SetBool("isWalking", true);
    }

    protected override void Detected() {
        Vector3 dir = (player.transform.position - rigidbody.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, q, Time.fixedDeltaTime * navMeshAgent.angularSpeed * 0.5f);
           
        if(timerManager.GetStatusOfTimer("ACD") <= 0) {
            enemyShooting?.Attack(transform.position);
            timerManager.ResetTimer("ACD");
        }

        GetComponent<Animator>().SetTrigger("Attack");

        if ((player.transform.position-transform.position).magnitude > playerDetectRange)
            SetState(EnemyStates.UNDETECTED);
    }
}
