using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI for displaying information and buttons when the game is paused.
/// </summary>
public class PauseUI : MonoBehaviour{

    [Header("References")]

    [SerializeField]
    RectTransform [] uiInGame;

    [SerializeField]
    RectTransform [] uiInPause;

    [SerializeField]
    RectTransform menuAfterDead;

    [SerializeField]
    CanvasGroup deathCanvasGroup;

    [SerializeField]
    RectTransform winScreen;

    [SerializeField]
    float timeToShowDeathScreen = 2f;

    [SerializeField]
    string menuScene;

    [SerializeField]
    string playerScene;

    bool isPause = false;
    bool isDead = false;
    float timer = 0;

    /// <summary>
    /// Initialization, starting the game.
    /// </summary>
    void Start(){
        isDead = false;
        PlayGame();
        LevelsController.Instance.OnEndGame += ShowWinScreen;
    }

    /// <summary>
    /// Shows or hides the pause menu when the Escape key is pressed.
    /// </summary>
    void LateUpdate(){
        if(Input.GetKeyDown(KeyCode.Escape) && !isDead){
            if(isPause){
                PlayGame();
            }else{
                PauseGame();
            }
        }
        
    }

    /// <summary>
    /// Resumes the game, shows the game UI and hides the pause UI.
    /// </summary>
    public void PlayGame(){
        isPause = false;
        Cursor.visible = false;
        Time.timeScale = 1;
        foreach(RectTransform t in uiInGame){
            t.gameObject.SetActive(true);
        }
        foreach(RectTransform t in uiInPause){
            t.gameObject.SetActive(false);
        }
        menuAfterDead.gameObject.SetActive(false);
        deathCanvasGroup.alpha = 0;
        winScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Pauses the game, hides the game UI and shows the pause UI.
    /// </summary>
    public void PauseGame(){
        isPause = true;
        Cursor.visible = true;
        Time.timeScale = 0;
        foreach(RectTransform t in uiInPause){
            t.gameObject.SetActive(true);
        }
        foreach(RectTransform t in uiInGame){
            t.gameObject.SetActive(false);
        }
        menuAfterDead.gameObject.SetActive(false);
        deathCanvasGroup.alpha = 0;
        winScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the menu informing about player's death.
    /// </summary>
    public void OnDead(){
        isDead = true;
        Cursor.visible = true;
        foreach(RectTransform t in uiInPause){
            t.gameObject.SetActive(false);
        }
        foreach(RectTransform t in uiInGame){
            t.gameObject.SetActive(false);
        }
        menuAfterDead.gameObject.SetActive(true);
        winScreen.gameObject.SetActive(false);
        StartCoroutine(DeathScreen());
    }

    /// <summary>
    /// Shows the menu informing about player's win.
    /// </summary>
    public void ShowWinScreen(){
        isDead = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        foreach(RectTransform t in uiInPause){
            t.gameObject.SetActive(false);
        }
        foreach(RectTransform t in uiInGame){
            t.gameObject.SetActive(false);
        }
        menuAfterDead.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Quits the game and shows the main menu.
    /// </summary>
    public void QuitToMenu() {
        SceneManager.LoadScene(menuScene);
    }

    /// <summary>
    /// Starts a new game.
    /// </summary>
    public void StartNewGame() {
        SceneManager.LoadScene(playerScene);
    }

    /// <summary>
    /// A coroutine for showing the death screen.
    /// </summary>
    /// <returns>coroutine for showing the death screen.</returns>
    IEnumerator DeathScreen(){
        Time.timeScale = 0.75f;
        while(timer <= timeToShowDeathScreen){          
            float val = Mathf.Clamp01(timer/timeToShowDeathScreen);
            deathCanvasGroup.alpha = val;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0f;
        
    }
    
}
