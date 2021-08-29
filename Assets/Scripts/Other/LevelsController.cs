using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour{

    public static LevelsController Instance { get; private set; }

    public System.Action BeforeChangeLevel;
    public System.Action AfterChangeLevel;

    [SerializeField]
    List<string> levels = new List<string>();

    [SerializeField]
    string playerScene = "player_scene";

    Scene currentLevelScene;
    string currentSceneLoaded;
    int level = 0;

    void Awake(){
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    void Start() {
        StartLevel();
    }

    public void StartLevel() {
        int level = 0;
        Scene sceneP = SceneManager.GetSceneByName(playerScene);
        if (!sceneP.isLoaded || !sceneP.IsValid()) {
            StartCoroutine(LoadScene(playerScene));
        }

        Scene levelScene = SceneManager.GetSceneByName(levels[level]);
        if (!levelScene.isLoaded || !levelScene.IsValid()) {
            Debug.Log("load level");
            StartCoroutine(ChangeLevel(level));
        }
    }

    public void GoLevelUp() {
        level++;
        if (level < levels.Count) {
            StartCoroutine(ChangeLevel(level));
        }
    }

    IEnumerator LoadScene(string nameScene, System.Action after = null) {
        yield return SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Additive);
        after?.Invoke();
    }

    IEnumerator ChangeLevel(int scene, System.Action after = null) {

        BeforeChangeLevel?.Invoke();

        if (scene >= 1)
            yield return SceneManager.UnloadSceneAsync(levels[scene - 1]);

        yield return SceneManager.LoadSceneAsync(levels[scene], LoadSceneMode.Additive);

        currentLevelScene = SceneManager.GetSceneByName(levels[scene]);
        SceneManager.SetActiveScene(currentLevelScene);

        RoomManager roomManager = FindObjectOfType<RoomManager>();

        if (roomManager)
            roomManager.Generate();

        after?.Invoke();
        AfterChangeLevel?.Invoke();
    }

}
