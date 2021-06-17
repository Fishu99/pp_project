using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public GameObject[] singleEntryRooms;
    public GameObject[] doubleEntryRooms;
    public GameObject[] tripleEntryRooms;


    public List<GameObject> GetRoomsWithProperEntry(int openingDir, int numberOfEntrances) {
        List<GameObject> rooms = new List<GameObject>();
        int indexOfEntry;
        switch(openingDir){
            case 1:
                indexOfEntry = 2;
                break;
            case 2:
                indexOfEntry = 1;
                break;
            case 3:
                indexOfEntry = 4;
                break;
            case 4:
                indexOfEntry = 3;
                break;
            default:
                Debug.LogError("Wrong openingDir value (valid range is 1-4), you're was " + openingDir.ToString());
                return null;
        }
        indexOfEntry--; // Arrays begins with 0

        /*This block will be more optimized in the future*/

        if (numberOfEntrances == 0) {
            rooms.Add(closedRoom);
        }
        else if (numberOfEntrances == 1) {
            foreach (GameObject template in singleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
        }
        else if (numberOfEntrances == 2) {
            foreach (GameObject template in doubleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
            foreach (GameObject template in singleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
        }
        else if (numberOfEntrances == 3) {
            foreach (GameObject template in tripleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
            foreach (GameObject template in doubleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
            foreach (GameObject template in singleEntryRooms) {
                Room roomInfo = template.GetComponent<Room>();
                bool[] roomEntrances = roomInfo.getEntryExists();
                if (roomEntrances[indexOfEntry] == true)
                    rooms.Add(template);
            }
        }
        else {
            Debug.LogError("Wrong numberOfEntrances. Valid range is 0-3, it was " + numberOfEntrances.ToString());
        }
        return rooms;
    }
}
