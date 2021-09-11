using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used for creating a 2D representation of the level. It contains
/// all the needed information of the room to spawn and moreover the GameObject room template.
/// </summary>
public class RoomMatrixStatus
{
    /// <summary>
    /// Is this room going to be spawned (some positions are not - limited number of rooms to spawn)
    /// </summary>
    /// <value>Information is this room will be spawned</value>
    public bool reservedForSpawn {get; private set;}
    /// <summary>
    /// RFU, is this room last from the origin branch (last by it's depth).
    /// </summary>
    /// <value>Information is this room last room from the origin branch</value>
    private bool lastFromOriginBranch {get; set;}
    /// <summary>
    /// GameObject template of the room what will be spawned.
    /// </summary>
    /// <value>GameObject template of the room to spawn</value>
    private GameObject roomTemplate {get; set;}
    /// <summary>
    /// Origin branch of the room (top, bottom, left, right)
    /// </summary>
    /// <value>Origin branch direction</value>
    private RoomDirection originBranch {get; set;}
    /// <summary>
    /// Position in roomSpawnMatrix, which is a 2D representation of the level.
    /// </summary>
    /// <value>Position of this room in roomSpawnMatrix</value>
    public Vector2Int mxPos {get; private set;}

    /// <summary>
    /// Constructor of this class.
    /// </summary>
    public RoomMatrixStatus() {
        reservedForSpawn = false;
        lastFromOriginBranch = false;
        roomTemplate = null;
    }
    /// <summary>
    /// Destructor of this class
    /// </summary>
    /// <value></value>
    ~RoomMatrixStatus() {}

    /// <summary>
    /// Parameterized constructor of RoomMatrixStatus.
    /// </summary>
    /// <param name="reservedForSpawn">Value of reservedForSpawn variable</param>
    /// <param name="lastFromOriginBranch">Value of lastFromOriginBranch variable</param>
    /// <param name="roomTemplate">Value of roomTemplate variable</param>
    /// <param name="originBranch">Value of originBranch variable</param>
    /// <param name="mxPos">Value of mxPos variable</param>
    public RoomMatrixStatus(bool reservedForSpawn, bool lastFromOriginBranch, GameObject roomTemplate, RoomDirection originBranch,Vector2Int mxPos) {
        this.reservedForSpawn = reservedForSpawn;
        this.lastFromOriginBranch = lastFromOriginBranch;
        this.roomTemplate = roomTemplate;
        this.originBranch = originBranch;
        this.mxPos = mxPos;
    }
    
    /// <summary>
    /// Access to GameObject template of the room.
    /// </summary>
    /// <returns>Template of the room to be spawned</returns>
    public GameObject getTemplate() {
        return roomTemplate;
    }

    /// <summary>
    /// Sets a template of the room to be spawned.
    /// </summary>
    /// <param name="newTemplate">GameObject template of the room</param>
    public void setTemplate(GameObject newTemplate) {
        roomTemplate = newTemplate;
    }

    /// <summary>
    /// Sets a status of the variable that defines if this room is going to be spawned in the level.
    /// </summary>
    /// <param name="status">Status of reservedForSpawn variable</param>
    public void setReservedForSpawn(bool status) {
        reservedForSpawn = status;
    }
}
