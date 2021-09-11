using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy's health bar. It displays the enemy's health from the related Health component.
/// </summary>
public class TestHealthBar : MonoBehaviour
{
    [SerializeField] private Health monitoredHealth;
    [SerializeField] GameObject barFill;
    private int previousHealth;
    
    void Start()
    {
        SetBarFill(1f);
    }

    /// <summary>
    /// In each Update the enemy's health is checked. If it changed, the health bar is updated.
    /// </summary>
    void Update()
    {
        if(monitoredHealth.CurrentHealth != previousHealth)
        {
            previousHealth = monitoredHealth.CurrentHealth;
            float healthFraction = (float)monitoredHealth.CurrentHealth / monitoredHealth.MaxHealth;

            if (!monitoredHealth.IsAlive()) {
                Destroy(gameObject);
            }

            SetBarFill(healthFraction);
        }
    }

    /// <summary>
    /// Sets the fill of the health bar.
    /// </summary>
    /// <param name="fill">a number between 0 and 1 describing how much of the bar should be filled.</param>
    private void SetBarFill(float fill)
    {
        //Debug.Log(fill);
        Vector3 oldScale = barFill.transform.localScale;
        Vector3 newScale = new Vector3(fill, oldScale.y, oldScale.z);
        barFill.transform.localScale = newScale;
    }
}
