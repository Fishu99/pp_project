using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for storage of the templates of the rooms.
/// </summary>
public class RoomBuilder : MonoBehaviour
{
    /// <summary>
    /// Templates of single entry rooms
    /// </summary>
    [SerializeField] private List<GameObject> tS_singleRooms;
    /// <summary>
    /// Templates of double entry rooms
    /// </summary>
    [SerializeField] private List<GameObject> tS_doubleRooms;
    /// <summary>
    /// Templates of triple entry rooms
    /// </summary>
    [SerializeField] private List<GameObject> tS_tripleRooms;
    /// <summary>
    /// Template of the entry room
    /// </summary>
    [SerializeField] private List<GameObject> tS_entryRooms;
    /// <summary>
    /// Templates of obstacles for single rooms
    /// </summary>
    [SerializeField] private List<GameObject> tS_obstacles;
    /// <summary>
    /// Template of the ending room
    /// </summary>
    [SerializeField] private List<GameObject> tS_endingRoom;

    /// <summary>
    /// Probability of particular type of the room spawn array.
    /// </summary>
    /// <value>Probability array</value>
    private float[,] probabilityTable {get;set;}

    /// <summary>
    /// Initializes probability array values.
    /// </summary>
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

    /// <summary>
    /// Gets a GameObject templates of the possible rooms, which contains required parameters.
    /// </summary>
    /// <param name="curDepth">Current depth of the room (starting from the entryRoom)</param>
    /// <param name="maxDepth">Maximum depth of the level rooms</param>
    /// <param name="reqDir">Required opening direction of the room</param>
    /// <param name="reqRoomType">Required room type</param>
    /// <returns>List of the rooms that matches the requirements</returns>
    public List<GameObject> GetRoomTemplates(int curDepth, int maxDepth, RoomDirection reqDir, int reqRoomType) {
        List<GameObject> roomTemplates = null;
        
        //RFU functionality - adding handle to different maxDepth than 4
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

        //Filter rooms by reqDir and reqRoomType (if it is equal to 0 - there isn't type filter)
        roomTemplates = FilterListOfRooms(roomTemplates, reqDir, reqRoomType);
        return roomTemplates;
    }

    /// <summary>
    /// Gets an obstacle GameObject for a particular room type.
    /// </summary>
    /// <param name="roomType">Type of the room</param>
    /// <returns>Template of the obstacle for a room</returns>
    public GameObject GetObstacle(int roomType) {
        if (roomType < 1 || roomType > 5) {
            Debug.LogError("There is no such type of the room");
            return null;
        }

        List<GameObject> validObstacles = new List<GameObject>();
        foreach (GameObject tmpObst in tS_obstacles)
        {
            Obstacle obstInfo = tmpObst.GetComponent<Obstacle>();
            if (obstInfo.getType() == roomType) {
                validObstacles.Add(tmpObst); //I won't change any of its parameters (so I don't instantiate)
            }
        }

        if (validObstacles.Count <= 0) {
            Debug.LogError("List of chosen obstacles was empty!");
            return null;
        }

        int idOfChosenObst = Random.Range(0, validObstacles.Count);
        return validObstacles[idOfChosenObst];
    }

    /// <summary>
    /// Filters given templates against the required direction of entry and the required room type
    /// </summary>
    /// <param name="roomTemplates">List of the tempaltes to filter</param>
    /// <param name="reqDir">Required entrance direction</param>
    /// <param name="reqRoomType">Required room type</param>
    /// <returns>List of the GameObject room templates that matches requirements</returns>
    private List<GameObject> FilterListOfRooms(List<GameObject> roomTemplates, RoomDirection reqDir, int reqRoomType) {
        List<GameObject> filteredList = new List<GameObject>();
        foreach (GameObject room in roomTemplates)
        {
            Room roomInfo = room.GetComponent<Room>();
            //roomInfo.entryExist[(int)reqDir]
            if(roomInfo.getEntryExist(reqDir)) {
                if (reqRoomType == 0 || reqRoomType == roomInfo.getRoomType())
                    filteredList.Add(room);
            }            
        }
        return filteredList;
    }

    /// <summary>
    /// Randomly selects a room from the list, spawns it and returns.
    /// </summary>
    /// <param name="roomTemplates">List of the templates</param>
    /// <returns>Selected and spawned room GameObject</returns>
    public GameObject GetTemplateFromList(List<GameObject> roomTemplates) {
        int rand = Random.Range(0, roomTemplates.Count);

        GameObject chosenRoom = Instantiate(roomTemplates[rand], Vector3.zero, Quaternion.identity);
        chosenRoom.name = roomTemplates[rand].name;
        chosenRoom.SetActive(false);
        
        return chosenRoom;
    }

    /// <summary>
    /// Gets a template of the room with required parameters.
    /// </summary>
    /// <param name="top">Is top entrance needed</param>
    /// <param name="bottom">Is the bottom entrance needed</param>
    /// <param name="left">Is left entrance needed</param>
    /// <param name="right">Is right entrance needed</param>
    /// <param name="reqRoomType">Required room type</param>
    /// <returns>GameObject room template</returns>
    public GameObject GetChosenTemplate(bool top, bool bottom, bool left, bool right, int reqRoomType) {
        int nbOfEntrances = (top ? 1:0) + (bottom ? 1:0) + (left ? 1:0) + (right ? 1:0);
        List<GameObject> validTemplates;

        int rand;
        GameObject validTemplate;
        switch(nbOfEntrances) {
            case 0:
                return null;
            case 1:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_singleRooms, reqRoomType);
                rand = Random.Range(0, validTemplates.Count);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name;
                return validTemplate;
            case 2:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_doubleRooms, reqRoomType);
                rand = Random.Range(0, validTemplates.Count);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name;
                return validTemplate;
            case 3:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_tripleRooms, reqRoomType);
                rand = Random.Range(0, validTemplates.Count);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplates[rand].name;
                return validTemplate;
            case 4:
                validTemplates = GetCompatibleRooms(top, bottom, left, right, tS_entryRooms, reqRoomType);
                rand = Random.Range(0, validTemplates.Count);
                validTemplate = Instantiate(validTemplates[rand], Vector3.zero, Quaternion.identity);
                validTemplate.name = validTemplate.name;
                return validTemplate;
            default:
                Debug.LogError("nbOfEntrances wasn't in 0-4 range!");
                return null;                         
        }
    }

    /// <summary>
    /// Filters GameObject templates list of rooms on the condition to be compatible with given parameters.
    /// </summary>
    /// <param name="top">Is top entrance needed</param>
    /// <param name="bottom">Is bottom entrance needed</param>
    /// <param name="left">Is left entrance needed</param>
    /// <param name="right">Is right entrance needed</param>
    /// <param name="roomTemplates">List of rooms to be filtered</param>
    /// <param name="reqRoomType">Required room type</param>
    /// <returns>List of compatible room GameObject templates</returns>
    private List<GameObject> GetCompatibleRooms(bool top, bool bottom, bool left, bool right, List<GameObject> roomTemplates, int reqRoomType) {
        List<GameObject> validTemplates = new List<GameObject>();
        foreach (GameObject room in roomTemplates)
        {
            Room roomInfo = room.GetComponent<Room>();
            if (roomInfo.getRoomType() == reqRoomType || reqRoomType == 0)
                if(roomInfo.getEntryExist(RoomDirection.top) == top)
                    if(roomInfo.getEntryExist(RoomDirection.bottom) == bottom)
                        if(roomInfo.getEntryExist(RoomDirection.left) == left)
                            if(roomInfo.getEntryExist(RoomDirection.right) == right)
                                validTemplates.Add(room);
        }
        return validTemplates;
    }

    /// <summary>
    /// Choses and spawns an ending room.
    /// </summary>
    /// <param name="dir">Required entrance direction</param>
    /// <param name="pos">Position of the ending room</param>
    /// <returns>Spawned ending room GameObject</returns>
    public GameObject GetEndingRoom(RoomDirection dir, Vector3 pos) {
        GameObject endRoomT = Instantiate(tS_endingRoom[(int)dir], pos, Quaternion.identity);
        endRoomT.name = tS_endingRoom[(int)dir].name + "/Portal";
        return endRoomT;
    }

    /// <summary>
    /// Returns a direction of a single entrance room.
    /// </summary>
    /// <param name="room">GameObject template of the room to check</param>
    /// <returns>RoomDirection of the entrance</returns>
    public RoomDirection GetDirOfSingleRoom(GameObject room) {
        Room roomInfo = room.GetComponent<Room>();
        if(roomInfo.getEntryAmount() != 1) {
            Debug.LogError("Invalid number of entrances to invoke this function!");
            return RoomDirection.none;
        }

        bool[] entryArray = roomInfo.getEntryExistArray();
        if(entryArray[(int)RoomDirection.top])
            return RoomDirection.top;
        else if (entryArray[(int)RoomDirection.bottom])
            return RoomDirection.bottom;
        else if (entryArray[(int)RoomDirection.left])
            return RoomDirection.left;
        else if (entryArray[(int)RoomDirection.right])
            return RoomDirection.right;
        else {
            Debug.LogError("Room should have at least one entrance!");
            return RoomDirection.none;
        }
    }

}
