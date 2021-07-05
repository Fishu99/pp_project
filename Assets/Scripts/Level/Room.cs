using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int entryAmount;
    [SerializeField] private bool[] entryExist;
    [SerializeField] private bool[] entryStatus;
    [SerializeField] private GameObject[] spawnPoints; //GameObjects with position for a new room to spawn

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

    public void setAllEntrancesOccupied() {
        entryStatus[0] = false;
        entryStatus[1] = false;
        entryStatus[2] = false;
        entryStatus[3] = false;
    }
}