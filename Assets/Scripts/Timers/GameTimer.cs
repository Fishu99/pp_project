using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float gameTime = 0.0f;
    [SerializeField] private bool isLocked = true;

    void Update()
    {
        if(isLocked == false) {
            gameTime += Time.deltaTime;
        }
    }

    public void StartTimer() {
        isLocked = false;
    }

    public void StopTimer() {
        isLocked = true;
    }

    public float GetRAWgameTime() {
        return gameTime;
    }

    public string GetSTRINGgameTime() {
        int minutes = (int)(gameTime / 60); //It will cut the value after comma
        int seconds = (int)(gameTime - minutes*60);
        string gTime = minutes.ToString() + " min. " + seconds.ToString() + " s.";
        return gTime;
    }
}
