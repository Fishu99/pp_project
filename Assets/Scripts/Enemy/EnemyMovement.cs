using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    UNDETECTED,
    DETECTED,
}

public abstract class EnemyMovement : MonoBehaviour
{
    [SerializeField] public GameObject weaponGrip;
    [SerializeField] public EnemyWaypoints waypointSystem;
    protected TimerManager timerManager;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rigidbody;

    protected EnemyMelee enemyMelee;
    protected EnemyShooting enemyShooting;
    private Health health;
    private Loottable loottable;
    [SerializeField] private float attackCooldownTime;


    [SerializeField] protected float waypointReachDistance;
    protected int myWaypointSystemID;
    
    
    //TODO: Wyluskiwanie z managera
    [SerializeField] public GameObject player;
    [SerializeField] protected float playerDetectRange;
    protected EnemyStates state;


    void Start()
    {
        GetTheComponents();
        InitValues();
    }

	protected void SetState(EnemyStates newstate)
	{
		switch (newstate)
		{
			case EnemyStates.UNDETECTED:
                UnsetStateDetectedValues();
                SetStateUndetectedValues();

				state = newstate;
				break;
			case EnemyStates.DETECTED:
                UnsetStateUndetectedValues();
                SetStateDetectedValues();

				state = newstate;
				break;
        }
    }

    void Update() {
        if (!health.IsAlive())
        {
            loottable.DropItems();
            Destroy(gameObject);
        }

        switch (state)
		{
			case EnemyStates.UNDETECTED:
				Undetected();
				break;
			case EnemyStates.DETECTED:
				Detected();
				break;
        }

        bool moving = navMeshAgent.velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;

        if(moving)
        {
            GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    private void GetTheComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        timerManager = GetComponent<TimerManager>();
        health = GetComponent<Health>();
        loottable = GetComponent<Loottable>();

        enemyShooting = GetComponent<EnemyShooting>();
        enemyMelee = GetComponent<EnemyMelee>();
    }



    private void InitValues() {
        myWaypointSystemID = 0;
        state = EnemyStates.UNDETECTED;

        TimerToZero destinationReachedCooldown = new TimerToZero(500f, 500f);
        destinationReachedCooldown.locked = true;
        timerManager.AddTimer("DRC", destinationReachedCooldown);

        TimerToZero attackCooldown = new TimerToZero(attackCooldownTime, 0f);
        attackCooldown.locked = false;
        timerManager.AddTimer("ACD", attackCooldown);

        SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
    }

    protected abstract void SetStateDetectedValues();
    protected abstract void UnsetStateDetectedValues();

    private void SetStateUndetectedValues() {
        
    }
    private void UnsetStateUndetectedValues() {
        timerManager.SetLock("DRC", true);
        timerManager.ResetTimer("DRC");
    }




    protected void SetDestination(EnemyWaypoint waypoint)
    {
        if(waypoint != null)
        {
            Vector3 destinationPosition = waypoint.transform.position;
            navMeshAgent?.SetDestination(destinationPosition);
        }
    }



    protected void Undetected() {
        if(
            waypointSystem.IsWaypointInReach(myWaypointSystemID, this.transform, waypointReachDistance) &&
            timerManager.IsTimerLocked("DRC")
        ) 
        {
            timerManager.SetLock("DRC", false);
        }
        else if(
            !timerManager.IsTimerLocked("DRC") &&
            timerManager.GetStatusOfTimer("DRC") <= 0f
        )
        {
            timerManager.SetLock("DRC", true);
            myWaypointSystemID++;
            myWaypointSystemID = myWaypointSystemID % waypointSystem.GetWaypointCount();
            SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
            timerManager.ResetTimer("DRC");
        }

        if((player.transform.position-transform.position).magnitude <= playerDetectRange)
            SetState(EnemyStates.DETECTED);
    }

    protected abstract void Detected();



    [SerializeField] protected bool debugGizmos;
    protected virtual void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, waypointReachDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        }
    }
}