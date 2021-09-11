using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enumeration of the enemy states.
/// </summary>
public enum EnemyStates
{
    UNDETECTED,
    DETECTED,
    SEARCHING
}

/// <summary>
/// A class controlling the enemy's movement.
/// </summary>
public abstract class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// An object marking the point where the enemy holds a weapon.
    /// </summary>
    [SerializeField] public GameObject weaponGrip;

    /// <summary>
    /// Waypoints for the enemy.
    /// </summary>
    [SerializeField] public EnemyWaypoints waypointSystem;

    /// <summary>
    /// A manager for enemy's timers.
    /// </summary>
    protected TimerManager timerManager;

    /// <summary>
    /// NavMeshAgent controlling the enemy's movement.
    /// </summary>
    protected NavMeshAgent navMeshAgent;

    /// <summary>
    /// Enemy's RigidBody.
    /// </summary>
    protected Rigidbody rigidbody;

    /// <summary>
    /// Enemy's EnemyMelee component.
    /// </summary>
    protected EnemyMelee enemyMelee;

    /// <summary>
    /// Enemy's EnemyShooting component.
    /// </summary>
    protected EnemyShooting enemyShooting;

    /// <summary>
    /// Enemy's Health component.
    /// </summary>
    private Health health;

    /// <summary>
    /// Enemy's Lootable component.
    /// </summary>
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

    /// <summary>
    /// State of the enemy.
    /// </summary>
    protected EnemyStates state;

    Collider collider;
    Animator animator;

    protected float timerToDetect;

    /// <summary>
    /// Last position of the player.
    /// </summary>
    protected Vector3 lastPositionOfPlayer;

    /// <summary>
    /// Gets the component references and initializes some values.
    /// </summary>
    void Start()
    {
        GetTheComponents();
        InitValues();
    }

    /// <summary>
    /// Changes the enemy's state.
    /// </summary>
    /// <param name="newstate">new state to set.</param>
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

    /// <summary>
    /// Tells if the enemy can see the player.
    /// </summary>
    /// <returns>true if the enemy can see the player.</returns>
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

    /// <summary>
    /// Enemy's Update.
    /// </summary>
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

    /// <summary>
    /// Gets the references to the enemy's components.
    /// </summary>
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


    /// <summary>
    /// Initializes enemy values.
    /// </summary>
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

    /// <summary>
    /// Method called when the enemy enters the DETECTED state.
    /// </summary>
    protected abstract void SetStateDetectedValues();

    /// <summary>
    /// Method called when the enemy exits the DETECTED state.
    /// </summary>
    protected abstract void UnsetStateDetectedValues();

    /// <summary>
    /// Method called when the enemy enters the UNDETECTED state.
    /// </summary>
    private void SetStateUndetectedValues() {
        
    }

    /// <summary>
    /// Method called when the enemy exits the UNDETECTED state.
    /// </summary>
    private void UnsetStateUndetectedValues() {
        timerManager.SetLock("DRC", true);
        timerManager.ResetTimer("DRC");
    }

    /// <summary>
    /// Sets the enemy's destination (the point where the enemy will walk).
    /// </summary>
    /// <param name="waypoint">The destination of the enemy.</param>
    protected void SetDestination(EnemyWaypoint waypoint)
    {
        if(waypoint != null && navMeshAgent != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
        {
            Vector3 destinationPosition = waypoint.transform.position;
            navMeshAgent?.SetDestination(destinationPosition);
        }
    }

    /// <summary>
    /// Enemy's behavior in the UNDETECTED state. The method is called in every Update if the enemy's state is UNDETECTED.
    /// </summary>
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

    /// <summary>
    /// Enemy's behavior in the DETECTED state. The method is called in every Update if the enemy's state is DETECTED.
    /// </summary>
    protected abstract void Detected();

    /// <summary>
    /// Enemy's behavior in the SEARCHING state. The method is called in every Update if the enemy's state is SEARCHING.
    /// </summary>
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

    /// <summary>
    /// When set to true, the gizmos will be shown in debug mode.
    /// </summary>
    [SerializeField] protected bool debugGizmos;

    /// <summary>
    /// Draws some wire sphere gizmos.
    /// One has radius equal to waypointReachDistance
    /// and the other one has a radius equal to playerDetectRange.
    /// </summary>
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