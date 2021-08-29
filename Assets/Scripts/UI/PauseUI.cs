using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start(){
        isDead = false;
        PlayGame();
    }

    void LateUpdate(){
        if(Input.GetKeyDown(KeyCode.Escape) && !isDead){
            if(isPause){
                PlayGame();
            }else{
                PauseGame();
            }
        }
        
    }

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

    public void ShowWinScreen(){
        isDead = true;
        Cursor.visible = true;
        Time.timeScale = 0.3f;
        foreach(RectTransform t in uiInPause){
            t.gameObject.SetActive(false);
        }
        foreach(RectTransform t in uiInGame){
            t.gameObject.SetActive(false);
        }
        menuAfterDead.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    public void QuitToMenu() {
        SceneManager.LoadScene(menuScene);
    }

    public void StartNewGame() {
        SceneManager.LoadScene(playerScene);
    }

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
