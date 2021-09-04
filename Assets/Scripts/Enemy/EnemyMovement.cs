using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    UNDETECTED,
    DETECTED,
    SEARCHING
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
    [SerializeField] private LayerMask detectMask;
    [SerializeField] private float minTimeDetect = 10f;


    [SerializeField] protected float waypointReachDistance;
    protected int myWaypointSystemID;
    
    
    //TODO: Wyluskiwanie z managera
    [SerializeField] public GameObject player;
    [SerializeField] protected float playerDetectRange;
    [SerializeField] protected float playerMaxCloseDistance = 3f;
    protected EnemyStates state;

    Collider collider;
    Animator animator;

    protected float timerToDetect;
    protected Vector3 lastPositionOfPlayer;

    void Start()
    {
        GetTheComponents();
        InitValues();
    }

	protected void SetState(EnemyStates newstate)
	{

        if (newstate == state) {
            return;
        }

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
            case EnemyStates.SEARCHING:
                lastPositionOfPlayer = player.transform.position;
                navMeshAgent?.SetDestination(lastPositionOfPlayer);
                state = newstate;
                break;
        }
    }

    protected bool CanSeePlayer() {

        if (player == null) {
            return false;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= playerMaxCloseDistance) {
            return true;
        }

        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out raycastHit, playerDetectRange, detectMask)) {
            Vector3 direction = (raycastHit.point - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if (raycastHit.transform.GetComponentInChildren<PlayerMovement>() != null && angle <= 90) {
                return true;
            }
        }

        return false;
    }

    void Update() {
        if (!health.IsAlive())
        {
            loottable.DropItems();
            collider.enabled = false;
            rigidbody.isKinematic = true;
            animator.SetTrigger("Die");
            rigidbody.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
            return;
            //Destroy(gameObject);
        }

        switch (state)
		{
			case EnemyStates.UNDETECTED:
				Undetected();
				break;
			case EnemyStates.DETECTED:
				Detected();
				break;
            case EnemyStates.SEARCHING:
                Searching();
                break;
        }

        bool moving = navMeshAgent.velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;

        if(moving)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void GetTheComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        timerManager = GetComponent<TimerManager>();
        health = GetComponent<Health>();
        loottable = GetComponent<Loottable>();
        collider = GetComponentInChildren<Collider>();
        animator = GetComponentInChildren<Animator>();

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

        if (enemyMelee != null){
            animator.SetFloat("Weapon", 0f);
        }else if (enemyShooting != null){
            animator.SetFloat("Weapon", enemyShooting.GetAnimTypeWeapon());
        }

        timerToDetect = 0f;
        health.onHit.AddListener(() => { if(this.state == EnemyStates.UNDETECTED)this.SetState(EnemyStates.SEARCHING); });

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
        if(waypoint != null && navMeshAgent != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
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
            timerManager.GetStatusOfTimer("DRC") <= 0f &&
            waypointSystem.GetWaypointCount() > 0
        )
        {
            timerManager.SetLock("DRC", true);
            myWaypointSystemID++;
            myWaypointSystemID = myWaypointSystemID % waypointSystem.GetWaypointCount();
            SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
            timerManager.ResetTimer("DRC");
        }

        if(CanSeePlayer())
            SetState(EnemyStates.DETECTED);
    }

    protected abstract void Detected();

    protected void Searching() {

        timerToDetect += Time.deltaTime;

        if (timerToDetect > minTimeDetect) {
            timerToDetect = 0f;
            SetState(EnemyStates.UNDETECTED);
            return;
        }

        if (CanSeePlayer())
            SetState(EnemyStates.DETECTED);

    }

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