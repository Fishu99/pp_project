using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A timer measuring the game time.
/// </summary>
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

    /// <summary>
    /// Starts a timer.
    /// </summary>
    public void StartTimer() {
        isLocked = false;
    }

    /// <summary>
    /// Stops a timer.
    /// </summary>
    public void StopTimer() {
        isLocked = true;
    }

    /// <summary>
    /// Gets the game time in seconds as a float.
    /// </summary>
    /// <returns>the game time as a float</returns>
    public float GetRAWgameTime() {
        return gameTime;
    }

    /// <summary>
    /// Gets the game time as a string in a format mm : ss.
    /// </summary>
    /// <returns>the game time as a string in a format mm : ss</returns>
    public string GetSTRINGgameTime() {
        int minutes = (int)(gameTime / 60); //It will cut the value after comma
        int seconds = (int)(gameTime - minutes*60);
        string gTime = minutes.ToString("D2") + " : " + seconds.ToString("D2") + "";
        return gTime;
    }
}
