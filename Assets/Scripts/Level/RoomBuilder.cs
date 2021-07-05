using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] private List<GameObject> tS_singleRooms;   //Templates of single entry rooms
    [SerializeField] private List<GameObject> tS_doubleRooms;   //Templates of double entry rooms
    [SerializeField] private List<GameObject> tS_tripleRooms;   //Templates of triple entry rooms
    [SerializeField] private GameObject tS_entryRoom;   //Template of the entry room


    private float[,] probabilityTable {get;set;}

    private void Awake() {
        /*  4 steps of probability which scales with maxDepth
            1# Beggining depth: {single, double, triple} x 'Rooms'
            2# Middle depth:    {single, double, triple} x 'Rooms'
            3# Greater depth:   {single, double, triple} x 'Rooms'
            4# Ending depth:    {single, double, triple} x 'Rooms'
        */
        probabilityTable = new float[4,3]{
            {0.1f, 0.2f, 0.7f},
            {0.1f, 0.3f, 0.6f},
            {0.3f, 0.4f, 0.3f},
            {0.6f, 0.35f, 0.05f}
        };
    }

    public List<GameObject> GetRoomTemplates(int curDepth, int maxDepth, RoomDirection reqDir) {
        List<GameObject> roomTemplates = null;
        
        //TODO functionality - add handle to different maxDepth than 4
        if (maxDepth != 4)
            return null;
        //Assuming that maxDepth is equal to 4
        float maxRange = 1000.0f;
        int depth = curDepth-1;
        if (depth < 0 || depth > 3) {
            Debug.LogError("Depth: " + depth);
            return null;
        }
        float chanceSingle = probabilityTable[curDepth-1,0]*maxRange;
        float chanceDouble = probabilityTable[curDepth-1,1]*maxRange;
        float chanceTriple = probabilityTable[curDepth-1,2]*maxRange;
        float randomNumber = Random.Range(0.0f, 1000.0f);

        // chanceSingle -> chanceDouble -> chanceTriple
        // 1 + 2 + 3 = 100%
        if (randomNumber < chanceSingle) {
            //Single rooms are chosen
            roomTemplates = new List<GameObject>(tS_singleRooms);
        }
        else if (randomNumber < chanceSingle + chanceDouble) {
            //Double rooms are chosen
            roomTemplates = new List<GameObject>(tS_doubleRooms);
        }
        else if (randomNumber < maxRange/*chanceSingle + chanceDouble + chanceTriple*/) {
            //Triple rooms are chosen
            roomTemplates = new List<GameObject>(tS_tripleRooms);
        }
        else {
            Debug.LogWarning("Catched an error, randomNumber was out of range");
        }

        //Filter rooms by reqDir
        roomTemplates = FilterListOfRooms(roomTemplates, reqDir);
        return roomTemplates;
    }

    private List<GameObject> FilterListOfRooms(List<GameObject> roomTemplates, RoomDirection reqDir) {
        List<GameObject> filteredList = new List<GameObject>();
        foreach (GameObject room in roomTemplates)
        {
            Room roomInfo = room.GetComponent<Room>();
            //roomInfo.entryExist[(int)reqDir]
            if(roomInfo.getEntryExist(reqDir)) {
                filteredList.Add(room);
            }            
        }

        // for(int i = roomTemplates.Count - 1; i >= 0; i--) {
        //     Room roomInfo = roomTemplates[i].GetComponent<Room>();
        //     if(roomInfo.entryExist[(int)reqDir] == false) {
        //         roomTemplates.Remove(roomTemplates[i]);
        //     }
        // }

        return filteredList;
    }

    public GameObject GetTemplateFromList(List<GameObject> roomTemplates) {
        int rand = Random.Range(0, roomTemplates.Count-1);

        GameObject chosenRoom = Instantiate(roomTemplates[rand], Vector3.zero, Quaternion.identity);
        chosenRoom.name = roomTemplates[rand].name + "_T";
        chosenRoom.SetActive(false);
        
        return chosenRoom;
    }

    public GameObject GetChosenTemplate(bool top, bool bottom, bool left, bool right) {
        int nbOfEntrances = (top ? 1:0) + (bottom ? 1:0) + (left ? 1:0) + (right ? 1:0);
        List<GameObject> validTemplates;

        int rand;
        GameObject validTemplate;
        switch(nbOfEntrances) {
            case 0:
                return null;
            case 1:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_singleRooms);
                rand = Random.Range(0, validTemplates.Count-1);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name + "_T";
                return validTemplate;
            case 2:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_doubleRooms);
                rand = Random.Range(0, validTemplates.Count-1);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name + "_T";
                return validTemplate;
            case 3:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_tripleRooms);
                rand = Random.Range(0, validTemplates.Count-1);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name + "_T";
                return validTemplate;
            case 4:
                validTemplate = Instantiate(tS_entryRoom, Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplate.name + "_T";
                return validTemplate;
            default:
                Debug.LogError("nbOfEntrances wasn't in 0-4 range!");
                return null;                         
        }
    }

    private List<GameObject> GetCompatibleRooms(bool top, bool bottom, bool left, bool right, List<GameObject> roomTemplates) {
        List<GameObject> validTemplates = new List<GameObject>();
        foreach (GameObject room in roomTemplates)
        {
            Room roomInfo = room.GetComponent<Room>();
            if(roomInfo.getEntryExist(RoomDirection.top) == top)
                if(roomInfo.getEntryExist(RoomDirection.bottom) == bottom)
                    if(roomInfo.getEntryExist(RoomDirection.left) == left)
                        if(roomInfo.getEntryExist(RoomDirection.right) == right)
                            validTemplates.Add(room);
        }
        return validTemplates;
    }

}
