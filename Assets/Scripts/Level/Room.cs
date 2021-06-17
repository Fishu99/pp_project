using UnityEngine;
public class Room : MonoBehaviour
{
    private int numberOfEntrances;
    [SerializeField] private bool[] entryStatus;    // True if opened, false when closed;
    [SerializeField] private bool[] entryExists;    // id: 0->1->2->3 means Top->Bottom->Left->Right

    public void setEntryStatus(int index, bool status) {
        entryStatus[index] = status;
    }

    public bool getEntryStatusByIndex(int index) {
        return entryStatus[index];
    }

    public int getNumberOfEntrances() {
        return numberOfEntrances;
    }

    public bool[] getEntryStatus() {
        return entryStatus;
    }

    public bool[] getEntryExists() {
        return entryExists;
    }

    void Awake () {
        UpdateNumberOfEntrances();
    }

    void Update () {
        //test function
        // Debug.Log (
        //     "RoomLeftCounter: " + Global.roomCounter.ToString() + 
        //     "\t RoomOpensCounter: " + Global.roomOpenCounter.ToString() + 
        //     "\t TestCounter: " + Global.testCounter
        // );
    }

    private void UpdateNumberOfEntrances() {
        int entranceCounter = 0;
        foreach (bool state in entryExists) {
            if (state == true)
                entranceCounter++;
        }
        numberOfEntrances = entranceCounter;
    }
}
