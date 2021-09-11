using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for displaying the player's health bar.
/// </summary>
public class HealthBarUI : MonoBehaviour{

    [Header("References")]

    [SerializeField]
    Image healthBar;

    [SerializeField]
    Image damageBar;

    [Header("Values")]

    [SerializeField]
    float smoothTime;

    [SerializeField]
    float timeToDown;

    float healthBarStatus = 1;
    float damageBarStatus = 1;

    float currentVelocity;

    float startWidth;

    float timer;
    float currentDamageBarStatus;

    void Awake(){
        healthBarStatus = 1;
        damageBarStatus = 1;
        startWidth = healthBar.rectTransform.sizeDelta.x;
    }

    void LateUpdate(){
        timer -= Time.deltaTime;
        currentDamageBarStatus = damageBarStatus;
        if(timer <= smoothTime && timer > 0){
            currentDamageBarStatus = Mathf.Lerp(healthBarStatus,damageBarStatus, timer/smoothTime);
        }else if(timer < 0){
            currentDamageBarStatus = damageBarStatus = healthBarStatus;
        }
        damageBar.rectTransform.sizeDelta = new Vector2(currentDamageBarStatus * startWidth, damageBar.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// Sets new health value to display.
    /// </summary>
    /// <param name="currentHealth">current value of player's health.</param>
    /// <param name="maxHealth">max value of player's health.</param>
    public void SetHealth(float currentHealth, float maxHealth){

        float currentHealthBarStatus = currentHealth / maxHealth;
        if(currentHealthBarStatus < healthBarStatus){
            timer = timeToDown + smoothTime;
            damageBarStatus = currentDamageBarStatus;
        }

        healthBarStatus = currentHealthBarStatus;
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarStatus * startWidth, healthBar.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// Sets the health bar to 50%.
    /// </summary>
    [ContextMenu("Test")]
    public void Set50PercentDamage(){
        SetHealth(50, 100);
    }

}
