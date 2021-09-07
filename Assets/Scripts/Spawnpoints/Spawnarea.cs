using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnarea : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnList;
    [SerializeField] private int maxSpawns;
    [SerializeField] private Vector3 spawnAreaSize;
    [SerializeField] private float spawnerCooldownTime;
    [SerializeField] private bool startSpawnerState;
    
    [SerializeField] private GameObject enemiesContainer;

    protected TimerManager timerManager;

    [SerializeField] protected EnemyWaypoints waypointSystemRef;
    [SerializeField] protected GameObject playerRef;

    public int spawnsLeft;

    void Start()
    {
        GetTheComponents();
        InitValues();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerManager.GetStatusOfTimer("SCD") <= 0) {
            timerManager.ResetTimer("SCD");
            if(spawnsLeft > 0) {
                spawnsLeft--;
                ForceSpawn();
            }
        }       
    }

    public void ForceSpawn() {
        float xspawn = Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2);
        float yspawn = Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2);
        float zspawn = Random.Range(-spawnAreaSize.z/2, spawnAreaSize.z/2);
        
        Vector3 newspawn = new Vector3(transform.position.x + xspawn, transform.position.y + yspawn, transform.position.z + zspawn);
        GameObject toSpawn = spawnList[Random.Range(0, spawnList.Count)];
        //Debug.Log("Spawned " + (transform.position.x + xspawn) + "/" + (transform.position.y + yspawn) + "/" + (transform.position.z + zspawn));

        GameObject spawned = Instantiate(toSpawn, newspawn, transform.rotation, enemiesContainer.transform) as GameObject;
        spawned.name = toSpawn.name;
        InitSpawnedGameObject(spawned);
    }

    private void GetTheComponents()
    {
        timerManager = GetComponent<TimerManager>();
    }

    private void InitValues() {
        spawnsLeft = maxSpawns;

        TimerToZero spawnerCooldown = new TimerToZero(spawnerCooldownTime, 1f);
        spawnerCooldown.locked = startSpawnerState;
        timerManager.AddTimer("SCD", spawnerCooldown);

        //For Benek to check - added for program to work properly
        if (playerRef == null)
            playerRef = GameObject.FindGameObjectWithTag("Player");

    }

    private void InitSpawnedGameObject(GameObject go) {
        EnemyMovement enemyMovement = go.GetComponent<EnemyMovement>();
        if(enemyMovement) {
            enemyMovement.waypointSystem = waypointSystemRef;
            enemyMovement.player = playerRef;
        }
    }



    //PRZY WCHODZENIU/WYCHOZENIU DO POKOJU
    public void StartSpawning() {
        timerManager.SetLock("SCD", false);
    }

    public void StopSpawning() {
        timerManager.SetLock("SCD", true);
    }


    [SerializeField] private bool debugGizmos;
    void OnDrawGizmos()
	{
        if(debugGizmos)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, spawnAreaSize.z));
        }
	}
}
