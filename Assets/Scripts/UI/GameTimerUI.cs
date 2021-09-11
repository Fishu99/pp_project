using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI for displaying the game timer.
/// </summary>
public class GameTimerUI : MonoBehaviour{

    /// <summary>
    /// Text displaying the timer.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI timerText;

    /// <summary>
    /// The timer whose value is displayed on th UI.
    /// </summary>
    [SerializeField]
    GameTimer gameTimer;

    /// <summary>
    /// Starts the timer.
    /// </summary>
    void OnEnable() {
        gameTimer.StartTimer();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    void OnDisable() {
        gameTimer.StopTimer();
    }

    /// <summary>
    /// Displays the current value of the timer.
    /// </summary>
    void Update() {
        timerText.text = gameTimer.GetSTRINGgameTime();
    }

}
