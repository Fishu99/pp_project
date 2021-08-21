using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealthBar : MonoBehaviour
{
    [SerializeField] private Health monitoredHealth;
    [SerializeField] GameObject barFill;
    private int previousHealth;
    
    void Start()
    {
        SetBarFill(1f);
    }

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

    private void SetBarFill(float fill)
    {
        //Debug.Log(fill);
        Vector3 oldScale = barFill.transform.localScale;
        Vector3 newScale = new Vector3(fill, oldScale.y, oldScale.z);
        barFill.transform.localScale = newScale;
    }
}
