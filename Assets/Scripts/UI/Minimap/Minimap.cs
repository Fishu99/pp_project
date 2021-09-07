using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{    
    [SerializeField] private GameObject miniRoomTemplate;
    [SerializeField] private GameObject miniRoomsContainer;
    private List<MiniRoom> miniRoomsList;

    private List<RoomMatrixStatus[]> rooms;
    private GameObject player;
    public Vector3 playerPos;
    public Vector2Int playerMxPos;
    private int maxLevelDepth = 4;

    private bool[,] minimapMatrix;
    private GameObject[,] minimapGObjMatrix;
    private Vector2Int startRoomPos;
    private int roomSize = 50;

    public void ConfigureMinimap(List<RoomMatrixStatus[]> roomsMatrix, GameObject player, int maxDepth, Vector2Int entryPos, Vector2Int endingPos) {
        this.rooms = roomsMatrix;
        this.player = player;
        this.maxLevelDepth = maxDepth;
        minimapMatrix = new bool[maxLevelDepth*2 + 1,maxLevelDepth*2 + 1];
        startRoomPos = new Vector2Int(maxLevelDepth, maxLevelDepth);
        miniRoomsList = new List<MiniRoom>();

        Vector2Int tMxPos;
        GameObject tRoom;
        bool[] tEntryExist;
        Vector3 tWorldPos;
        int tMRoomStatus;


        foreach (RoomMatrixStatus[] row in rooms)
        {
            foreach (RoomMatrixStatus room in row)
            {
                if (room != null) {
                    tMxPos = room.mxPos;
                    tRoom = room.getTemplate();
                    Room roomInfo = tRoom.GetComponent<Room>();
                    tEntryExist = roomInfo.getEntryExistArray();
                    tWorldPos = GetCordsArrayToPos(tMxPos.x, tMxPos.y);
                    tMRoomStatus = 1; //Undiscovered as a standard color

                    GameObject newMiniRoom = Instantiate(miniRoomTemplate, tWorldPos, Quaternion.identity, miniRoomsContainer.transform);
                    newMiniRoom.name = miniRoomTemplate.name + "_" + tMxPos;
                    
                    MiniRoom miniRoomInfo = newMiniRoom.GetComponent<MiniRoom>();
                    miniRoomInfo.SetMiniRoomProperties(tMxPos,tRoom,tEntryExist,tWorldPos,tMRoomStatus);
                    miniRoomsList.Add(miniRoomInfo);
                }
                else {
                    Debug.Log("null?");
                }
            }
        };
        
        //Set starting and ending rooms valid parameters
        MiniRoom sRoom = FindMiniRoom(entryPos);
        MiniRoom eRoom = FindMiniRoom(endingPos);

        sRoom.SetMiniRoomStatus("isStartingRoom", true);
        eRoom.SetMiniRoomStatus("isPortalRoom", true);

        // based on created structure manage objects:
        // checking position of the player, and showing results on the minimap
    }

    private void FixedUpdate() {
        CheckPlayerPosition();
    }

    private void CheckPlayerPosition() {
        Vector3 playerPos = player.transform.position;
        this.playerPos = playerPos;
        float pPosX = playerPos.x;
        float pPosZ = playerPos.z;
        float halfRS = roomSize/2;
        float tAbsPos;

        //Handling X axis
        int mxPPosX;
        tAbsPos = Mathf.Abs(pPosX);
        if (tAbsPos < halfRS)
            mxPPosX = startRoomPos.x;
        else {
            int cordX = (int)((tAbsPos-halfRS)/roomSize) + 1;
            mxPPosX = pPosX > 0 ? startRoomPos.x + cordX : startRoomPos.x - cordX;
        }


        //Handling Z axis
        int mxPPosZ;
        tAbsPos = Mathf.Abs(pPosZ);
        if (tAbsPos < halfRS)
            mxPPosZ = startRoomPos.y;
        else {
            int cordZ = (int)((tAbsPos-halfRS)/roomSize) + 1;
            mxPPosZ = pPosZ > 0 ? startRoomPos.y - cordZ : startRoomPos.y + cordZ; // It's actually startRoomPos.z in matrix format
        }
        
        //Vector2Int presentCords = new Vector2Int(mxPPosX, mxPPosZ);
        Vector2Int presentCords = new Vector2Int(mxPPosZ, mxPPosX);

        if (playerMxPos != presentCords) {
            //Update mini rooms
            MiniRoom playerRoom = FindMiniRoom(presentCords);
            MiniRoom previousRoom = FindMiniRoom(playerMxPos);

            //Valid properties:
            // - isDiscovered
            // - isPortalRoom
            // - isStartingRoom
            // - isPlayerInIt
            
            if(previousRoom != null)
                previousRoom.SetMiniRoomStatus("isPlayerInIt", false);
            if(playerRoom != null) {
                playerRoom.SetMiniRoomStatus("isPlayerInIt", true);
                playerRoom.SetMiniRoomStatus("isDiscovered", true);
            }
            
            playerMxPos = presentCords;
        }       
    }

    private MiniRoom FindMiniRoom (Vector2Int pos) {
        Vector2Int wantedMxPos = pos;
        for (int i = 0; i < miniRoomsList.Count; i++) {
            MiniRoom miniRoom = miniRoomsList[i];
            if (miniRoom.GetMxPos() == wantedMxPos) {
                return miniRoom;
            }
        }
        return null;
    }


    private void SpawnMiniRooms() {
        int maxLength = maxLevelDepth*2;
        int counter = 1;
        for (int x = 0; x < maxLength; x++) {
            for (int y = 0; y < maxLength; y++) {
                if (minimapMatrix[x,y] == true) {
                    Vector2 pos = GetCordsArrayToPos(x,y);
                    GameObject newMini = Instantiate(miniRoomTemplate, new Vector3(pos.x,0,pos.y), Quaternion.identity, transform);
                    newMini.name = miniRoomTemplate.name + "_" + counter.ToString();
                    counter++;
                }
            }
        }
    }

    private Vector3 GetCordsArrayToPos(int x, int y) {
        Vector3 result = new Vector3(-x+maxLevelDepth, 0, -(y-maxLevelDepth));
        return result;
    }

    private Vector2 GetCordsPosToArray(Vector3 pos) {
        Vector2 result = new Vector2(-pos.x+maxLevelDepth, pos.z+maxLevelDepth);
        return result;
    }
}
