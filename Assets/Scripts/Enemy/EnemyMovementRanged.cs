using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementRanged : EnemyMovement
{
    protected override void SetStateDetectedValues() {
        
    }
    
    protected override void UnsetStateDetectedValues() {
        
    }

    protected override void Detected() {
        Vector3 dir = (player.transform.position - rigidbody.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        rigidbody.transform.rotation = Quaternion.RotateTowards(rigidbody.transform.rotation, q, Time.fixedDeltaTime * navMeshAgent.angularSpeed * 0.1f);

        if((player.transform.position-transform.position).magnitude > playerDetectRange)
            SetState(EnemyStates.UNDETECTED);
    }
}
