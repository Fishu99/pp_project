using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for managing the game's progress.
/// </summary>
public class LevelsController : MonoBehaviour{

    /// <summary>
    /// The singleton instance of the class.
    /// </summary>
    public static LevelsController Instance { get; private set; }

    /// <summary>
    /// Action performed before the level is changed.
    /// </summary>
    public System.Action BeforeChangeLevel;

    /// <summary>
    /// Action performed after the level is changed.
    /// </summary>
    public System.Action AfterChangeLevel;

    /// <summary>
    /// Action performed when the game is finished.
    /// </summary>
    public System.Action OnEndGame;

    /// <summary>
    /// Level names.
    /// </summary>
    [SerializeField]
    List<string> levels = new List<string>();

    [SerializeField]
    string playerScene = "player_scene";

    /// <summary>
    /// Current scene
    /// </summary>
    Scene currentLevelScene;
    string currentSceneLoaded;

    /// <summary>
    /// Number of the current level.
    /// </summary>
    int level = 0;

    /// <summary>
    /// Creates the singleton instance if not created before.
    /// </summary>
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
            StartCoroutine(ChangeLevel(level));
        }
    }

    public void GoLevelUp() {
        level++;
        if (level < levels.Count) {
            StartCoroutine(ChangeLevel(level));
        } else {
            OnEndGame?.Invoke();
        }
    }

    /// <summary>
    /// Returns the game progress.
    /// </summary>
    /// <returns>the game progress between 0 and 1.</returns>
    public float GetProgress() {
        return (float)level / levels.Count;
    }

    /// <summary>
    /// Loads a scene.
    /// </summary>
    /// <param name="nameScene">name of the scene to load</param>
    /// <param name="after">the action to be executed after the load is complete</param>
    /// <returns>the coroutine loading the scene</returns>
    IEnumerator LoadScene(string nameScene, System.Action after = null) {
        yield return SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Additive);
        after?.Invoke();
    }

    /// <summary>
    /// Loads a new level.
    /// </summary>
    /// <param name="scene">the number of level to load</param>
    /// <param name="after">the action to be executed when the level is loaded.</param>
    /// <returns>the coroutine loading the level</returns>
    IEnumerator ChangeLevel(int scene, System.Action after = null) {

        BeforeChangeLevel?.Invoke();

        if (scene >= 1)
            yield return SceneManager.UnloadSceneAsync(levels[scene - 1]);

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if(playerMovement!= null)
            playerMovement.transform.position = Vector3.zero;

        if(scene >= 1)
            yield return new WaitForSecondsRealtime(2f);

        if (playerMovement != null)
            playerMovement.transform.position = Vector3.zero;

        yield return SceneManager.LoadSceneAsync(levels[scene], LoadSceneMode.Additive);

        currentLevelScene = SceneManager.GetSceneByName(levels[scene]);
        SceneManager.SetActiveScene(currentLevelScene);

        RoomManager roomManager = FindObjectOfType<RoomManager>();

        if (roomManager)
            roomManager.Generate();

        after?.Invoke();
        AfterChangeLevel?.Invoke();
    }

    public int GetLevelCount() {
        return levels.Count;
    }

    public int GetCurrentLevelID() {
        return level;
    }

    public void BonusLevelFailure() {
        level++;
    }

}
