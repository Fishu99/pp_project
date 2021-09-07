using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRoom : MonoBehaviour
{
    [SerializeField] private GameObject topEntry;
    [SerializeField] private GameObject bottomEntry;
    [SerializeField] private GameObject leftEntry;
    [SerializeField] private GameObject rightEntry;
    [SerializeField] private MeshRenderer mshRenderer;
    [SerializeField] private Material[] materials;

    
    private bool[] entryExist;  //Info about room entrances
    private GameObject room;    //Room GameObject for collecting info about room
    private Room roomInfo;  //Information about the room (collectibles and enemies)
    private Vector2Int mxPos;   //Position of miniRoom in matrix
    private Vector3 worldPos;   //Where to spawn miniRoom
    private bool isDiscovered = false;  //Was room discovered by the player?
    private bool isPortalRoom = false;  //Is portal in this room?
    private bool isStartingRoom = false;    //Is this a starting room?
    private bool isPlayerInIt = false;  //Is the player in it right now?
    private bool isEnemiesNearby = true;   //Are there enemies in the room?
    private bool isCollectiblesInRoom = true; //Are there collectibles in the room?
    private int mRoomStatus = 0; // 0_PlayerInRoom, 1_MysteryRoom, 2_Discovered, 3_Start, 4_Portal, 5_EnemiesNearby, 6_CollectiblesInRoom, 7_EmptyRoom
    
    void Start()
    { 
        entryExist = new bool[4];   // 0-top, 1-bottom, 2-left, 3-right
    }

    void Update()
    {
        //Checking validity of status
        if (mRoomStatus < 0)
            mRoomStatus = 0;
        else if (mRoomStatus > 7)
            mRoomStatus = 7;

        //Set appropriate material for the miniRoom
        mshRenderer.material = materials[mRoomStatus];  
    }

    private void FixedUpdate() {        
        //Checking amount of enemies and collectibles in the room
        //if (isCollectiblesInRoom) {
            List<GameObject> collectibles = roomInfo.GetRoomCollectibles();
            if (collectibles != null) {
                int counter = 0;
                for (int i = 0; i < collectibles.Count; i++) {
                    if (collectibles[i] != null)
                        counter++; 
                }
                if (counter == 0)
                    isCollectiblesInRoom = false;
                else
                    isCollectiblesInRoom = true;
                    // if (collectibles == null || collectibles.Count == 0)
                    //     isCollectiblesInRoom = false;
                    // else
                    //     isCollectiblesInRoom = true;    
            }
            
       // }
        //if (isEnemiesNearby) {
            GameObject enemiesContainer = roomInfo.GetEnemiesContainer();
            if (enemiesContainer != null) {                
                List<GameObject> enemies = new List<GameObject>();
                Transform parent = enemiesContainer.transform;

                for(int i=0; i < parent.childCount; i++) {
                    Transform child = parent.GetChild(i);
                    Health enemyHP = child.GetComponent<Health>();
                    if (enemyHP.IsAlive())
                        enemies.Add(child.gameObject);
                }       

                if(enemies == null || enemies.Count == 0)
                    isEnemiesNearby = false;
                else
                    isEnemiesNearby = true;
            }
       // }
    }

    public void SetMiniRoomProperties(Vector2Int mxPos, GameObject room, bool[] entryExist, Vector3 worldPos, int mRoomStatus) {
        this.mxPos = mxPos;
        this.room = room;
        this.entryExist = entryExist;
        this.worldPos = worldPos;
        this.mRoomStatus = mRoomStatus;

        topEntry.gameObject.SetActive(entryExist[0]);
        bottomEntry.gameObject.SetActive(entryExist[1]);
        leftEntry.gameObject.SetActive(entryExist[2]);
        rightEntry.gameObject.SetActive(entryExist[3]);

        this.roomInfo = this.room.GetComponent<Room>();
    }

    public void SetMiniRoomStatus(string boolName, bool value) {
        switch(boolName) {
            case "isDiscovered":
                isDiscovered = value;
                break;
            case "isPortalRoom":
                isPortalRoom = value;
                break;
            case "isStartingRoom":
                isStartingRoom = value;
                break;
            case "isPlayerInIt":
                isPlayerInIt = value;
                break;
            case "isEnemiesNearby":
                isEnemiesNearby = value;
                break;
            case "isCollectiblesInRoom":
                isCollectiblesInRoom = value;
                break;
            default:
                Debug.LogError("There is no such status as " + boolName);
                break;
        }

        UpdateMiniRoom();
    }

    private void UpdateMiniRoom() {
        // 0_PlayerInRoom, 1_MysteryRoom, 2_Discovered, 3_Start, 4_Portal, 5_EnemiesNearby, 6_CollectiblesInRoom, 7_EmptyRoom
        if(isPlayerInIt) {
            mRoomStatus = 0;
        }
        else if (isDiscovered) {
            if(isPortalRoom)
                mRoomStatus = 4;
            else if(isStartingRoom)
                mRoomStatus = 3;
            else if(isEnemiesNearby) {
                if(isCollectiblesInRoom)
                    mRoomStatus = 2;
                else
                    mRoomStatus = 5;
            }
            else if(isCollectiblesInRoom) {
                mRoomStatus = 6;
            }
            else
                mRoomStatus = 7;
                
        }
        else {
            mRoomStatus = 1;
        }
    }

    public Vector2Int GetMxPos() {
        return mxPos;
    }
}
