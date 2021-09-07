using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int entryAmount;
    [SerializeField] private bool[] entryExist;
    [SerializeField] private bool[] entryStatus;
    [SerializeField] private GameObject[] spawnPoints; //GameObjects with position for a new room to spawn
    [SerializeField] private int roomType;  //Type of room material

    private List<GameObject> collectibles;
    private GameObject enemiesContainer;

    public void setStatus(int index, bool status) {
        entryStatus[index] = status;
    }

    public bool getStatus(int index) {
        return entryStatus[index];
    }

    public bool[] getEntryStatusArray() {
        return entryStatus;
    }

    public bool[] getEntryExistArray() {
        return entryExist;
    }

    public bool getEntryExist(RoomDirection dir){
        return entryExist[(int)dir];
    }

    public GameObject[] getSpawnPointsArray() {
        return spawnPoints;
    }

    public int getEntryAmount() {
        return entryAmount;
    }

    public int getRoomType() {
        return roomType;
    }

    public void setAllEntrancesOccupied() {
        entryStatus[0] = false;
        entryStatus[1] = false;
        entryStatus[2] = false;
        entryStatus[3] = false;
    }

    public void SetCollectiblesList(List<GameObject> spawnedCollectibles) {
        collectibles = spawnedCollectibles;
    }

    public List<GameObject> GetRoomCollectibles() {
        return collectibles;
    }

    public GameObject GetEnemiesContainer() {
        return enemiesContainer;
    }

    public void FindEnemiesContainer() {
        Transform parent = transform;
        string searchTag = "EnemiesContainer";
        bool result = GetChildObject(parent, searchTag);
        if (result == false)
            Debug.LogWarning("Enemies container wasn't found");
    }

    private bool GetChildObject(Transform parent, string searchTag) {
        for(int i=0; i < parent.childCount; i++) {
            Transform child = parent.GetChild(i);
            if(child.tag == searchTag) {
                enemiesContainer = child.gameObject;
                return true;
            }
            if (child.childCount > 0) {
                bool result = GetChildObject(child, searchTag);
                if (result)
                    return true;
            }
        }
        return false;        
    }
}