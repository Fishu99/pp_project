using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public GameObject weaponGrip;
    [SerializeField] private EnemyWaypoints waypointSystem;
    private TimerManager timerManager;
    [SerializeField] private float waypointReachDistance;
    private int myWaypointSystemID;
    private Rigidbody rigidbody;

    //TODO: Wyluskiwanie z managera
    [SerializeField] private GameObject player;
    [SerializeField] private float playerDetectRange;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        GetTheComponents();
        InitValues();
    }

    void Update() {
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

        //TODO: Inna akcja w zaleznosci od broni, to jest do celow testowych
        if((player.transform.position-transform.position).magnitude <= playerDetectRange)
            navMeshAgent.isStopped = true;
        else
            navMeshAgent.isStopped = false;
    }

    private void GetTheComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        timerManager = GetComponent<TimerManager>();
    }

    private void InitValues() {
        myWaypointSystemID = 0;

        TimerToZero destinationReachedCooldown = new TimerToZero(500f, 500f);
        destinationReachedCooldown.locked = true;
        timerManager.AddTimer("DRC", destinationReachedCooldown);

        SetDestination(waypointSystem.GetWaypoint(myWaypointSystemID));
    }

    private void SetDestination(EnemyWaypoint waypoint)
    {
        if(waypoint != null)
        {
            Vector3 destinationPosition = waypoint.transform.position;
            navMeshAgent.SetDestination(destinationPosition);
        }
    }

    [SerializeField] private bool debugGizmos;
    void OnDrawGizmos() {
        if(debugGizmos)
		{
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, waypointReachDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        }
    }
}