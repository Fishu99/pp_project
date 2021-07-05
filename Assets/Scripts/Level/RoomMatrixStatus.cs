using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMatrixStatus
{
    public bool reservedForSpawn {get; private set;}
    private bool lastFromOriginBranch {get; set;}
    private GameObject roomTemplate {get; set;}
    private RoomDirection originBranch {get; set;}
    public Vector2Int mxPos {get; private set;}

    public RoomMatrixStatus() {
        reservedForSpawn = false;
        lastFromOriginBranch = false;
        roomTemplate = null;
    }
    ~RoomMatrixStatus() {}
    public RoomMatrixStatus(bool reservedForSpawn, bool lastFromOriginBranch, GameObject roomTemplate, RoomDirection originBranch,Vector2Int mxPos) {
        this.reservedForSpawn = reservedForSpawn;
        this.lastFromOriginBranch = lastFromOriginBranch;
        this.roomTemplate = roomTemplate;
        this.originBranch = originBranch;
        this.mxPos = mxPos;
    }
    
    public GameObject getTemplate() {
        return roomTemplate;
    }

    public void setTemplate(GameObject newTemplate) {
        roomTemplate = newTemplate;
    }

    public void setReservedForSpawn(bool status) {
        reservedForSpawn = status;
    }
}
