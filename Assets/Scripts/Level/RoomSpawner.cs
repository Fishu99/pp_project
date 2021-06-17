using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 -> needs a bottom door
    // 2 -> needs a top door
    // 3 -> needs a right door
    // 4 -> needs a left door

    private RoomTemplates templates;
    [SerializeField] private bool roomSpawned = false;
    [SerializeField] private float waitTime = 0.1f;

    void Start() {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Global.roomOpenCounter++;
        Invoke("SpawnRoom", waitTime);
        Global.testCounter++;
    }

    void SpawnRoom() {
        if (roomSpawned == false) {
            GameObject roomTemplate = GetRoomGameObject(openingDirection);
            if (roomTemplate == null)
                return;
            GameObject newRoom = Instantiate(roomTemplate, transform.position, Quaternion.identity);
            Room roomInfo = newRoom.GetComponent<Room>();
            if(roomInfo.getNumberOfEntrances() != 0){
                Global.roomOpenCounter -= 2;    // 2 because the room has a spawnpoint on itself too
                Global.roomCounter--;     
            }
            roomSpawned = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("SpawnPoint")) {
            RoomSpawner obj = other.GetComponent<RoomSpawner>();
            if (obj != null && obj.roomSpawned == false && roomSpawned == false) {
                // Block walls spawn 
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Global.roomOpenCounter -= 2;
            }
            roomSpawned = true;            
        }
    }

    GameObject GetRoomGameObject (int openingDir) {
        int maxEntrances = GetProperRoomEntrances();
        List<GameObject> roomTemplatesList = templates.GetRoomsWithProperEntry(openingDir, maxEntrances);
        if (roomTemplatesList == null || roomTemplatesList.Count == 0)
            return null;
        int randomIndex = Random.Range(0, roomTemplatesList.Count-1);   // -1 because array starts with 0 not 1
        GameObject roomTemplate = roomTemplatesList[randomIndex];

        return roomTemplate;
    }

    int GetProperRoomEntrances () {
        int maxEntrances;
        if (Global.roomCounter > 0) {
            if (Global.roomCounter - 2 >= Global.roomOpenCounter)    // -2 because its max value of new entrances of each room
                maxEntrances = 3;
            else if (Global.roomCounter - 1 >= Global.roomOpenCounter)   // -1 because its max value of 2EntrancesRoom
                maxEntrances = 2;
            else if (Global.roomCounter  <= Global.roomOpenCounter ) // Only 1 Entrances rooms will spawn
                maxEntrances = 1;
            else
                maxEntrances = 0;   // Room without entrance will be spawned
        }
        else {
            maxEntrances = 0;
        }


        /*
        Future Improvement:
            Only 3 arrays with templates, each one containing 1 or 2 or 3 entrance rooms (not mixed)
            and proper GameObject (still random) will be chosen by random index in range from 0 to (max1 + max2 + max3 length)
            Index  will point the proper GameObject (room template).
        */

        return maxEntrances;
    }
    
}
