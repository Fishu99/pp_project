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

    bool isPause = false;

    void Start(){
        PlayGame();
    }

    void LateUpdate(){
        if(Input.GetKeyDown(KeyCode.Escape)){
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
    }

    public void ChangeScene(int index){
        SceneManager.LoadScene(index);
    }
    
}
