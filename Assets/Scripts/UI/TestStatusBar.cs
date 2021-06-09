using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestStatusBar : MonoBehaviour
{
    [SerializeField] private GameObject ammunitionDisplay;
    [SerializeField] private GameObject reloadDisplay;
    [SerializeField] private GameObject player;
    private PlayerShooting playerShooting;
    private TMP_Text ammunitionText;
    private TMP_Text reloadText;

    void Start()
    {
        playerShooting = player.GetComponent<PlayerShooting>();
        ammunitionText = ammunitionDisplay.GetComponent<TMP_Text>();
        reloadText = reloadDisplay.GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        string ammunitionStr = "Ammunition: " + playerShooting.GetAmmunition();
        string reloadStr = "Shots before reload: " + playerShooting.GetShotsBeforeReload();
        ammunitionText.text = ammunitionStr;
        reloadText.text = reloadStr;
    }
}
