using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenTesting : MonoBehaviour
{	
    void Update () {
		if (Input.GetKeyDown("r")){//reload scene, for testing purposes
            
            Global.roomCounter = Global.roomCounterCopy;
            Global.roomOpenCounter = 0;
            Global.testCounter = 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
		}
	}

}
