using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static RoomDirection;

public class RoomManager : MonoBehaviour
{
    private List<GameObject> rooms;
    public List<RoomMatrixStatus[]> roomSpawnArray; //Matrix of rooms to spawn
    Vector2Int entryPos;
    [SerializeField] private GameObject entryRoom;
    
    private RoomBuilder roomBuilder;   //Builder for getting templates of appropriate room
    [SerializeField] private int roomAmount = 15; //Number of rooms to spawn
    private int[] roomPerBranchAmount;
    [SerializeField] private int maxRPathDepth = 4; //Max distance from entry room for room to spawn

    private const int spawnPointProximity = 50;
    private List<Vector2Int> mxPosRooms;
    private GameObject endingRoom;
    [SerializeField] private List<GameObject> collectibles;
    [SerializeField] private int reqRoomType = 0; //What type of rooms will be spawned (0 means MIX of types)






    // Start is called before the first frame update
    private void Start()
    {
        CheckArguments();
        InitializeParamaters();
        GenerateSpawnMatrix();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void CheckArguments() {
        //Checking possibility of spawning max number of rooms (DepthCheck)
        int matrixSize = maxRPathDepth*2 + 1; // +1 because of entry room
        int possibleRoomSpawns = (matrixSize*matrixSize) - 1;
        if (possibleRoomSpawns < roomAmount) {
            int minDepth;
            do {                
                minDepth = maxRPathDepth + 1;
                matrixSize = minDepth*2 + 1;
                possibleRoomSpawns = (matrixSize*matrixSize) - 1;
            } while (possibleRoomSpawns < roomAmount);

            Debug.LogWarning("It's not possible to spawn " + roomAmount + 
            " room(s) with depth of " + maxRPathDepth +
            ". Depth should be equal to at least " + minDepth + "."
            );
            maxRPathDepth = minDepth;
        }
        
        //Checking valid number of rooms (min 1 from each branch)
        if (roomAmount < 4) {
            Debug.LogError("Not enought rooms to spawn (min = 4, you're was = " + roomAmount + ")");
        }

    }

    private void InitializeParamaters() {
        roomSpawnArray = new List<RoomMatrixStatus[]>();
        int matrixLength = 2 * maxRPathDepth + 1;
        for (int i = 0; i < matrixLength; i++) {
            roomSpawnArray.Add(new RoomMatrixStatus[matrixLength]);
        }
        roomBuilder = this.transform.gameObject.GetComponent<RoomBuilder>();
        entryPos = new Vector2Int(maxRPathDepth, maxRPathDepth);
        roomSpawnArray[entryPos.x][entryPos.y] = new RoomMatrixStatus(false, false, entryRoom, none, entryPos);
        rooms = new List<GameObject>();
        mxPosRooms = new List<Vector2Int>();
        endingRoom = null;
        InitRPerBAmount();
    }

    //Creates room per branch counters
    private void InitRPerBAmount() {
        roomPerBranchAmount = new int[4];   //All elements are 0
        roomPerBranchAmount[(int)top]++;
        roomPerBranchAmount[(int)bottom]++;
        roomPerBranchAmount[(int)left]++;
        roomPerBranchAmount[(int)right]++;

        int maxAmountLeft = roomAmount - 4;

        List<int> branches = new List<int>();
        branches.Add(0);
        branches.Add(1);
        branches.Add(2);
        branches.Add(3);   

        int branch;
        int rand;
        int index;
        for (int i=0; i<3 ; i++) {
            index = Random.Range(0, branches.Count);
            branch = branches[index];
            rand = Random.Range(0, maxAmountLeft);
            roomPerBranchAmount[branch] += rand;
            maxAmountLeft -= rand;
            branches.RemoveAt(index);
        }
        index = 0;
        branch = branches[index]; //Last branch (which wasn't selected)
        rand = maxAmountLeft;
        roomPerBranchAmount[branch] += rand;
        maxAmountLeft -= rand;
        branches.RemoveAt(index);

        Debug.Log("Top: " + roomPerBranchAmount[(int)top]);
        Debug.Log("Bottom: " + roomPerBranchAmount[(int)bottom]);
        Debug.Log("Left: " + roomPerBranchAmount[(int)left]);
        Debug.Log("Right: " + roomPerBranchAmount[(int)right]);
        Debug.Log("Sum: " + (roomPerBranchAmount[(int)right] + roomPerBranchAmount[(int)top] + roomPerBranchAmount[(int)bottom] + roomPerBranchAmount[(int)left]));
    }

    private void GenerateSpawnMatrix(){
        List<int> randomNumbers = new List<int>();
        randomNumbers.Add((int)top);
        randomNumbers.Add((int)bottom);
        randomNumbers.Add((int)left);
        randomNumbers.Add((int)right);

        bool[] entryChecked = {false, false, false, false};
        int entrancesLeft;
        do {
            entrancesLeft = 
                (entryChecked[(int)top] ? 0:1) +
                (entryChecked[(int)bottom] ? 0:1) +
                (entryChecked[(int)left] ? 0:1) +
                (entryChecked[(int)right] ? 0:1);
            if (entrancesLeft != 0) {
                //TODO maybe convert to RoomDir enum
                //int randomValue = Random.Range(1, entrancesLeft) - 1;
                int randomIndex = Random.Range(0, randomNumbers.Count);
                int randomValue = randomNumbers[randomIndex];
                randomNumbers.RemoveAt(randomIndex);

                Vector2Int pos;
                switch(randomValue) {
                    case 0: //Top branch is chosen
                        pos = new Vector2Int(entryPos.x - 1, entryPos.y);
                        FillMatrix(entryRoom, top, top, 1, pos, entryChecked);
                        entryChecked[(int)top] = true;
                        break;
                    case 1: //Bottom branch is chosen
                        pos = new Vector2Int(entryPos.x + 1, entryPos.y);
                        FillMatrix(entryRoom, bottom, bottom, 1, pos, entryChecked);
                        entryChecked[(int)bottom] = true;
                        break;
                    case 2: //Left branch is chosen
                        pos = new Vector2Int(entryPos.x, entryPos.y - 1);
                        FillMatrix(entryRoom, left, left, 1, pos, entryChecked);
                        entryChecked[(int)left] = true;
                        break;
                    case 3: //Right branch is chosen
                        pos = new Vector2Int(entryPos.x, entryPos.y + 1);
                        FillMatrix(entryRoom, right, right, 1, pos, entryChecked);
                        entryChecked[(int)right] = true;
                        break;
                }
            }
        } while (entrancesLeft > 0);
        
        DebugList();
        ValidateMatrix();
        SpawnRooms();
        TellMeWhichOnesAreMissing();
        FindAndSelectEndingRoom();
        GenerateNavMeshForLevel();
    }

    private void DebugList() {
        
        List<string> rmsFromList = new List<string>();
        foreach (RoomMatrixStatus[] x in roomSpawnArray)
        {
            foreach (RoomMatrixStatus y in x)
            {
                if (y != null) {
                    string info = "Type: " + y.getTemplate().name + ", pos: " + y.mxPos;
                    rmsFromList.Add(info);
                }
                else {
                    Debug.Log("null?");
                }
            }
        }
        Debug.Log("Pos that are not null: " + rmsFromList.Count);
    }

    private void ValidateMatrix() {
        int matrixLength = 2 * maxRPathDepth + 1;
        for(int x=0; x<matrixLength; x++) {
            for(int y=0; y<matrixLength; y++) {
                if (roomSpawnArray[x][y] != null && roomSpawnArray[x][y].reservedForSpawn) {
                    RoomMatrixStatus mxStats = roomSpawnArray[x][y];
                    GameObject roomGameObj = mxStats.getTemplate();
                    Room curRoom = roomGameObj.GetComponent<Room>();
                    bool[] entryExists = curRoom.getEntryExistArray();
                    bool[] entryStats = curRoom.getEntryStatusArray();

                    bool[] rEntry = new bool[4];    //Entry statuses for replacement room
                    
                    //Checking entrances for a new room - if necessary
                    //When equal means there is an open entry (not attached to any room) 
                    rEntry[(int)top] = (entryExists[(int)top] == entryStats[(int)top]) ? false : true;
                    rEntry[(int)bottom] = (entryExists[(int)bottom] == entryStats[(int)bottom]) ? false : true;
                    rEntry[(int)left] = (entryExists[(int)left] == entryStats[(int)left]) ? false : true;
                    rEntry[(int)right] = (entryExists[(int)right] == entryStats[(int)right]) ? false : true; 

                    bool isChangeNeeded = false;
                    foreach (bool sEntry in rEntry) {
                        if (sEntry == false){
                            isChangeNeeded = true;
                            break;
                        }
                    }

                    if(isChangeNeeded) {
                        //Room must be reassigned
                        GameObject newTemplate = roomBuilder.GetChosenTemplate(rEntry[(int)top], rEntry[(int)bottom], rEntry[(int)left], rEntry[(int)right], reqRoomType);
                        if (newTemplate == null) {
                            Debug.LogError("newTemplate is null!");
                        }
                        Destroy(roomGameObj);

                        Room newRoomInfo = newTemplate.GetComponent<Room>();
                        newRoomInfo.setAllEntrancesOccupied();
                        newTemplate.SetActive(false);

                        mxStats.setTemplate(newTemplate);
                    }
                }            
            }
        }
    }

    private void FillMatrix(GameObject prevRoom, RoomDirection entryDir, RoomDirection originBranch, int curDepth, Vector2Int pos, bool[] entryChecked) {
        //Check if there is anything left to spawn        
        if (roomAmount <= 0){
            return; //No more rooms to spawn
        }

        //Checking maximum amount of rooms per branch
        if (roomPerBranchAmount[(int)originBranch] <= 0) {
            return;
        }

        if (CheckBranch(pos, originBranch) == false) {
            return; //This room cannot spawn on another branches place.
        }

        //Check if the place to spawn room is empty
        if (roomSpawnArray[pos.x][pos.y] != null || IsPositionInTheList(pos, mxPosRooms)){
            return;
        }
        
        //Room will spawn at defined pos
        roomAmount--; //Due to creating a new one (this number means how many left)
        roomPerBranchAmount[(int)originBranch]--;
        bool roomAmountChanged = true; 

        mxPosRooms.Add(pos);
        Debug.Log("RoomName: " + prevRoom.transform.gameObject.name + " -> Next pos: " + pos);

        Room prevRoomInfo = prevRoom.GetComponent<Room>();

        if(entryDir == top){ //We spawn room on the top so we need room with bottom door
            //Getting template
            List<GameObject> roomTemplates = roomBuilder.GetRoomTemplates(curDepth, maxRPathDepth, bottom, reqRoomType);
            GameObject template = roomBuilder.GetTemplateFromList(roomTemplates);   //Randomise template
            if (template == null){
                Debug.LogError("Template was null");
                if (roomAmountChanged)
                    roomAmount++;
                return;
            }            

            //Updating parameters
            Room roomInfo = template.GetComponent<Room>();            
            roomInfo.setStatus((int)bottom, false);
            prevRoomInfo.setStatus((int)top, false);           

            //Checking spawned entrances
            CheckSpawnedEntrances(entryDir, roomInfo, template, originBranch, pos, entryChecked);

            //Finalization of filling matrix            
            roomSpawnArray[pos.x][pos.y] = new RoomMatrixStatus(true, false, template, originBranch, pos);
        }
        else if (entryDir == bottom) {//We spawn room on the bottom so we need room with top door
            //Getting template
            List<GameObject> roomTemplates = roomBuilder.GetRoomTemplates(curDepth, maxRPathDepth, top, reqRoomType);
            GameObject template = roomBuilder.GetTemplateFromList(roomTemplates);   //Randomise template
            if (template == null){
                Debug.LogError("Template was null");
                if (roomAmountChanged)
                    roomAmount++;
                return;
            }            

            //Updating parameters
            Room roomInfo = template.GetComponent<Room>();            
            roomInfo.setStatus((int)top, false);
            prevRoomInfo.setStatus((int)bottom, false);            

            //Checking spawned entrances
            CheckSpawnedEntrances(entryDir, roomInfo, template, originBranch, pos, entryChecked);

            //Finalization of filling matrix            
            roomSpawnArray[pos.x][pos.y] = new RoomMatrixStatus(true, false, template, originBranch, pos);
        }
        else if (entryDir == left) {//We spawn room on the left so we need room with right door
            //Getting template
            List<GameObject> roomTemplates = roomBuilder.GetRoomTemplates(curDepth, maxRPathDepth, right, reqRoomType);
            GameObject template = roomBuilder.GetTemplateFromList(roomTemplates);   //Randomise template
            if (template == null){
                Debug.LogError("Template was null");
                if (roomAmountChanged)
                    roomAmount++;
                return;
            }            

            //Updating parameters
            Room roomInfo = template.GetComponent<Room>();            
            roomInfo.setStatus((int)right, false);
            prevRoomInfo.setStatus((int)left, false);

            //Checking spawned entrances
            CheckSpawnedEntrances(entryDir, roomInfo, template, originBranch, pos, entryChecked);

            //Finalization of filling matrix
            roomSpawnArray[pos.x][pos.y] = new RoomMatrixStatus(true, false, template, originBranch, pos);
        }
        else if (entryDir == right) {//We spawn room on the right so we need room with left door
            //Getting template
            List<GameObject> roomTemplates = roomBuilder.GetRoomTemplates(curDepth, maxRPathDepth, left, reqRoomType);
            GameObject template = roomBuilder.GetTemplateFromList(roomTemplates);   //Randomise template
            if (template == null){
                Debug.LogError("Template was null");
                if (roomAmountChanged)
                    roomAmount++;
                return;
            }            

            //Updating parameters
            Room roomInfo = template.GetComponent<Room>();            
            roomInfo.setStatus((int)left, false);
            prevRoomInfo.setStatus((int)right, false);

            //Checking spawned entrances
            CheckSpawnedEntrances(entryDir, roomInfo, template, originBranch, pos, entryChecked);

            //Finalization of filling matrix
            roomSpawnArray[pos.x][pos.y] = new RoomMatrixStatus(true, false, template, originBranch, pos);
        }  
    }

    private void CheckSpawnedEntrances(RoomDirection prevEntry, Room roomInfo, GameObject rmTemplate, RoomDirection originBranch, Vector2Int pos, bool[] entryChecked) {
        if (prevEntry == top) {
            FillSpawnedEntrances(roomInfo, rmTemplate, top, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, left, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, right, originBranch, pos, entryChecked);
        }
        else if (prevEntry == bottom) {
            FillSpawnedEntrances(roomInfo, rmTemplate, bottom, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, left, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, right, originBranch, pos, entryChecked);
        }
        else if (prevEntry == left) {
            FillSpawnedEntrances(roomInfo, rmTemplate, top, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, bottom, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, left, originBranch, pos, entryChecked);
        }
        else if (prevEntry == right) {
            FillSpawnedEntrances(roomInfo, rmTemplate, top, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, bottom, originBranch, pos, entryChecked);
            FillSpawnedEntrances(roomInfo, rmTemplate, right, originBranch, pos, entryChecked);
        }
    }
    private void FillSpawnedEntrances(Room roomInfo, GameObject rmTemplate, RoomDirection eDir, RoomDirection originBranch, Vector2Int pos, bool[] entryChecked) {
        if (roomInfo.getEntryExist(eDir)) {
            Vector2Int nextPos = new Vector2Int(); // Must be assigned because of CS0165 error
            switch(eDir) {
                case top:
                    nextPos = new Vector2Int(pos.x - 1, pos.y);
                    break;
                case bottom:
                    nextPos = new Vector2Int(pos.x + 1, pos.y);
                    break;
                case left:
                    nextPos = new Vector2Int(pos.x, pos.y - 1);
                    break;
                case right:
                    nextPos = new Vector2Int(pos.x, pos.y + 1);
                    break;                
            }

            if (nextPos != entryPos) {
                Vector2Int depth = new Vector2Int(Mathf.Abs(nextPos.x - entryPos.x), Mathf.Abs(nextPos.y - entryPos.y));
                int nextDepth = depth.x > depth.y ? depth.x : depth.y;
                if (nextDepth <= maxRPathDepth){ //Ending zone
                    FillMatrix(rmTemplate, eDir, originBranch, nextDepth, nextPos, entryChecked);
                }   
            }
        }
    }

    private void ChooseObstacleForRoom(GameObject roomTemplate) {
        if (roomTemplate == null) {
            Debug.LogError("RoomTemplate was null when choosing an obstacle for it!");
            return;
        }
        Room roomInfo = roomTemplate.GetComponent<Room>();
        GameObject obstacleTemplate = roomBuilder.GetObstacle(roomInfo.getRoomType());

        if (obstacleTemplate != null) {
            GameObject spawnedObstacle = Instantiate(obstacleTemplate, roomTemplate.transform.position, Quaternion.identity);

            Vector3 position = spawnedObstacle.transform.position;
            spawnedObstacle.transform.position = Vector3.zero;

            spawnedObstacle.transform.rotation = Quaternion.Euler(0.0f, 90.0f * Random.Range(0,3), 0.0f);
            spawnedObstacle.transform.position = position;

            SpawnItemsInTheRoom(spawnedObstacle);

            spawnedObstacle.transform.SetParent(roomTemplate.transform);   
        }
     
    }

    private void SpawnItemsInTheRoom(GameObject obstacleGO) {     
        GameObject itemSpawnPts = null;
        Transform parent = obstacleGO.transform;
        for (int i = 0; i < parent.childCount; i++) {
            Transform child = parent.GetChild(i);
            if (child.tag == "ItemSP")
            {
                itemSpawnPts = child.gameObject;
                break;
            }
        }
        if (itemSpawnPts == null) {
            Debug.Log("There was no itemSP in the obstacle!");
            return;
        }

        for(int i = 0; i < itemSpawnPts.transform.childCount; i++) {
            Transform itemSP = itemSpawnPts.transform.GetChild(i);
            int chanceOfSpawn = Random.Range(0, 100);
            if(chanceOfSpawn > 50) {
                int idNo = Random.Range(0, collectibles.Count);
                Vector3 collectiblePos = itemSP.position;
                collectiblePos.y += 0.5f;
                GameObject newCollectible = Instantiate(collectibles[idNo], collectiblePos, Quaternion.identity);
                newCollectible.name = collectibles[idNo].name;
                newCollectible.transform.SetParent(obstacleGO.transform);
            }            
        }
    }

    private void SpawnRooms() {
        int matrixLength = 2 * maxRPathDepth + 1;
        for(int x=0; x<matrixLength; x++) {
            for(int y=0; y<matrixLength; y++) {
                if (roomSpawnArray[x][y] != null && roomSpawnArray[x][y].reservedForSpawn) {
                    Debug.Log("Spawning");
                    int difX = y - entryPos.y;  //It is X axis accordingly (not Y)
                    int difZ = entryPos.x - x;  //It is Z axis accordingly (not X)

                    float distanceX = difX * spawnPointProximity;
                    float distanceZ = difZ * spawnPointProximity;

                    Vector3 entryRoomPos = entryRoom.transform.position;
                    Vector3 newRoomPos = new Vector3(entryRoomPos.x + distanceX, entryRoomPos.y, entryRoomPos.z + distanceZ);
                    GameObject newRoomTemplate = roomSpawnArray[x][y].getTemplate();
                    GameObject newRoom = Instantiate(newRoomTemplate, newRoomPos, Quaternion.identity);
                    ChooseObstacleForRoom(newRoom);

                    //Destroy template obj
                    Destroy(newRoomTemplate);
                    roomSpawnArray[x][y].setTemplate(newRoom);

                    newRoom.SetActive(true);
                    rooms.Add(newRoom);
                    roomSpawnArray[x][y].setReservedForSpawn(false);
                }
            }
        }
        Debug.Log("Rooms:" + rooms.Count);              
    }

    private bool IsPositionInTheList(Vector2Int pos, List<Vector2Int> mxPos) {
        if(mxPos == null)
            return false;
        foreach (Vector2Int curPos in mxPos)
        {
            if (curPos.x == pos.x && curPos.y == pos.y)
                return true;
        }
        return false;
    }

    private bool CheckBranch(Vector2Int pos, RoomDirection originBranch) {
        bool rSpawnAvailable = true;

        if(pos.x == entryPos.x) {
            //We need to check Left and Right branches
            if (pos.y < entryPos.y) {
                //Left branch
                if (originBranch != left)
                    rSpawnAvailable = false;
            }
            else {
                //Right branch
                if (originBranch != right)
                    rSpawnAvailable = false;                
            }
        }
        else if (pos.y == entryPos.y) {            
            //We need to check Top and Bottom branches
            if (pos.x < entryPos.x) {
                //Top branch
                if (originBranch != top)
                    rSpawnAvailable = false;                
            }
            else {
                //Bottom branch
                if (originBranch != bottom)
                    rSpawnAvailable = false;                
            }
        }

        return rSpawnAvailable;
    }

    private void TellMeWhichOnesAreMissing() {
        int matrixLength = 2 * maxRPathDepth + 1;
        List<Vector2Int> missingRooms = new List<Vector2Int>();
        for(int x=0; x<matrixLength; x++) {
            for(int y=0; y<matrixLength; y++) {
                if (roomSpawnArray[x][y] != null) {
                    Vector2Int curPos = new Vector2Int(x,y);
                    bool isFound = false;
                    foreach (Vector2Int pos in mxPosRooms)
                    {
                        if (curPos.x == pos.x && curPos.y == pos.y) {
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound == false) {
                        missingRooms.Add(curPos);
                    }
                }
            }
        }
        string mRPositions = "";
        foreach (Vector2Int pos in missingRooms)  
        {
            mRPositions += pos + ", ";
        }
        Debug.Log("MissRoomPos: " + mRPositions);    
    }

    private void FindAndSelectEndingRoom() {
        List<RoomMatrixStatus> mxRooms = new List<RoomMatrixStatus>();

        //Searching for max depth
        bool mustKeepGoing = true;

        int depthCnt = 0;
        int matrixLength = 2 * maxRPathDepth; // +1-1 = 0 - array handler fix        
        int roomsAdded = 0;
        //For horizontal iteration
        int iStartOff;
        int iEndOff;
        //For vertical iteration
        int iStartOff2;
        int iEndOff2;

        while(mustKeepGoing) {
            //Setting important variables
            iStartOff = depthCnt;
            iEndOff = matrixLength-iStartOff;
            iStartOff2 = iStartOff + 1;
            iEndOff2 = iEndOff - 1;
            roomsAdded = 0;

            //Matrix always have the middle point (entry room)
            if (iStartOff == iEndOff) {
                break;  //Entry room cannot be ending room
            } 

            //Searching for rooms within a rectangular shape (whole one depth search)
            for(int i = iStartOff; i < iEndOff; i++) {
                if (roomSpawnArray[iStartOff][i] != null){
                    mxRooms.Add(roomSpawnArray[iStartOff][i]);
                    roomsAdded++;
                }
                if (roomSpawnArray[iEndOff][i] != null) {
                    mxRooms.Add(roomSpawnArray[iEndOff][i]);
                    roomsAdded++;
                }

                //This statement removes hazard of adding duplicated rooms (4hazards each loop)
                if (i >= iStartOff2 && i <= iEndOff2) {
                    if (roomSpawnArray[i][iStartOff] != null){
                        mxRooms.Add(roomSpawnArray[i][iStartOff]);
                        roomsAdded++;
                    }
                    if (roomSpawnArray[i][iEndOff] != null) {
                        mxRooms.Add(roomSpawnArray[i][iEndOff]);
                        roomsAdded++;
                    }
                }                    
            }

            //Checking results of the search
            if (roomsAdded > 0)
                mustKeepGoing = false;
            else
                depthCnt++;
        }

        //Selected room must be a one entry room
        mxRooms = CheckEndRoomPossibility(mxRooms);
        roomsAdded = mxRooms.Count;    

        //Checking and handling errors
        if (mustKeepGoing)
            Debug.LogError("There is only entry room in the matrix!");
        if (roomsAdded == 0)
            Debug.LogError("No rooms were added to the list!");
        if (mxRooms == null)
            Debug.LogError("mxRooms was null!");

        //Selecting the ending room randomly
        int iChosenRoom = Random.Range(0, roomsAdded);
        endingRoom = mxRooms[iChosenRoom].getTemplate();    //Getting GameObject
        //Spawning a proper template for ending room (with portal)
        Vector3 endRoomPos = endingRoom.transform.position;
        RoomDirection roomDir = roomBuilder.GetDirOfSingleRoom(endingRoom);
        GameObject endingRoomTmpl = roomBuilder.GetEndingRoom(roomDir, endRoomPos);
        Destroy(endingRoom);
        endingRoom = endingRoomTmpl;    //TODO: Reassign this template in room list

        Debug.Log("RoomsAdded: " + roomsAdded + " ChosenOneCords: " + mxRooms[iChosenRoom].mxPos);        
    }

    private List<RoomMatrixStatus> CheckEndRoomPossibility(List<RoomMatrixStatus> mxRooms) {
        List<RoomMatrixStatus> newMxRooms = new List<RoomMatrixStatus>();
        foreach (RoomMatrixStatus mxElement in mxRooms)
        {
            Room roomInfo = mxElement.getTemplate().GetComponent<Room>();
            if (roomInfo.getEntryAmount() == 1)
                newMxRooms.Add(mxElement);
        }
        return newMxRooms;
    }

    private void GenerateNavMeshForLevel() {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
}
