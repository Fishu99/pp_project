using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimerUI : MonoBehaviour{

    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    GameTimer gameTimer;

    void OnEnable() {
        gameTimer.StartTimer();
    }

    void OnDisable() {
        gameTimer.StopTimer();
    }

    void Update() {
        timerText.text = gameTimer.GetSTRINGgameTime();
    }

}
