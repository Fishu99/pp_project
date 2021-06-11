using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestStatusBar : MonoBehaviour
{
    [SerializeField] private GameObject ammunitionDisplay;
    [SerializeField] private GameObject reloadDisplay;
    [SerializeField] private GameObject healthDisplay;
    [SerializeField] private GameObject player;
    private PlayerShooting playerShooting;
    private Health playerHealth;
    private TMP_Text ammunitionText;
    private TMP_Text reloadText;
    private TMP_Text healthText;

    void Start()
    {
        playerShooting = player.GetComponent<PlayerShooting>();
        playerHealth = player.GetComponent<Health>();
        Debug.Log(playerShooting);
        ammunitionText = ammunitionDisplay.GetComponent<TMP_Text>();
        reloadText = reloadDisplay.GetComponent<TMP_Text>();
        healthText = healthDisplay.GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        string ammunitionStr = "Ammunition: " + playerShooting.GetAmmunition();
        string reloadStr = "Shots before reload: " + playerShooting.GetShotsBeforeReload();
        int healthPrecentage = (int)(100f * playerHealth.CurrentHealth / playerHealth.MaxHealth);
        string healthStr = "Health: " + healthPrecentage + "%";
        ammunitionText.text = ammunitionStr;
        reloadText.text = reloadStr;
        healthText.text = healthStr;
    }
}
