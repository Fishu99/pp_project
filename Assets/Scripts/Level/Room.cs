using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for containing information about a particular room.
/// </summary>
public class Room : MonoBehaviour
{
    /// <summary>
    /// Number of entrances.
    /// </summary>
    [SerializeField] private int entryAmount;
    /// <summary>
    /// Array of the entrances, if true then the entrance exist.
    /// [0] - top, [1] - bottom, [2] - left, [3] - right
    /// </summary>
    [SerializeField] private bool[] entryExist;
    /// <summary>
    /// Array of the entrances statuses, if false then entrance is occupied or doesnt exist.
    /// </summary>
    [SerializeField] private bool[] entryStatus;
    /// <summary>
    /// GameObjects with position for a new room to spawn.
    /// </summary>
    [SerializeField] private GameObject[] spawnPoints; 
    /// <summary>
    /// Type of room material.
    /// </summary>
    [SerializeField] private int roomType;
    /// <summary>
    /// All collectibles that are placed in the room.
    /// </summary>
    private List<GameObject> collectibles;
    /// <summary>
    /// Container that groups all enemies GameObjects within the room
    /// </summary>
    private GameObject enemiesContainer;

    /// <summary>
    /// Sets status of the entry by passed index of entrance and its status.
    /// </summary>
    /// <param name="index">Index of the entrance</param>
    /// <param name="status">Status of the entrance</param>
    public void setStatus(int index, bool status) {
        entryStatus[index] = status;
    }

    /// <summary>
    /// Access to status of the particular entrance.
    /// </summary>
    /// <param name="index">Index of the entrance</param>
    /// <returns>Status of the entrance</returns>
    public bool getStatus(int index) {
        return entryStatus[index];
    }

    /// <summary>
    /// Access to entrance status array.
    /// </summary>
    /// <returns>Entrance status array</returns>
    public bool[] getEntryStatusArray() {
        return entryStatus;
    }

    /// <summary>
    /// Access to entrance exist array.
    /// </summary>
    /// <returns>Entrance exist array</returns>
    public bool[] getEntryExistArray() {
        return entryExist;
    }

    /// <summary>
    /// Access to information if room has an entrance in some given direction.
    /// </summary>
    /// <param name="dir">Direction to check</param>
    /// <returns>Check result</returns>
    public bool getEntryExist(RoomDirection dir){
        return entryExist[(int)dir];
    }

    /// <summary>
    /// Access to spawn points of the surrounding rooms.
    /// </summary>
    /// <returns>Array of the spawn points</returns>
    public GameObject[] getSpawnPointsArray() {
        return spawnPoints;
    }

    /// <summary>
    /// Access to number of the entrances to the room.
    /// </summary>
    /// <returns>Number of the entrances in the room</returns>
    public int getEntryAmount() {
        return entryAmount;
    }

    /// <summary>
    /// Access to the type of the room.
    /// </summary>
    /// <returns>Type of the room</returns>
    public int getRoomType() {
        return roomType;
    }

    /// <summary>
    /// Sets all entrances occupied.
    /// </summary>
    public void setAllEntrancesOccupied() {
        entryStatus[0] = false;
        entryStatus[1] = false;
        entryStatus[2] = false;
        entryStatus[3] = false;
    }

    /// <summary>
    /// Sets a given list to collectibles variable.
    /// </summary>
    /// <param name="spawnedCollectibles">List of collectibles in the room</param>
    public void SetCollectiblesList(List<GameObject> spawnedCollectibles) {
        collectibles = spawnedCollectibles;
    }

    /// <summary>
    /// Access to list of the room collectibles.
    /// </summary>
    /// <returns>List of collectibles</returns>
    public List<GameObject> GetRoomCollectibles() {
        return collectibles;
    }

    /// <summary>
    /// Access to enemies container GameObject.
    /// </summary>
    /// <returns>Enemies container</returns>
    public GameObject GetEnemiesContainer() {
        return enemiesContainer;
    }

    /// <summary>
    /// Searches and sets enemies container GameObject to a variable.
    /// </summary>
    public void FindEnemiesContainer() {
        Transform parent = transform;
        string searchTag = "EnemiesContainer";
        bool result = GetChildObject(parent, searchTag);
        if (result == false)
            Debug.LogWarning("Enemies container wasn't found");
    }

    /// <summary>
    /// Function that searches through all parent childs GameObjects for an object with given tag and
    /// sets it to a class variable. Function used for searching for enemies container.
    /// </summary>
    /// <param name="parent">Transform of a GameObject parent</param>
    /// <param name="searchTag">Searched tag</param>
    /// <returns>Check result</returns>
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