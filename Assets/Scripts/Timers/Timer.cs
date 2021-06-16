using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Timer
{
    protected float restartValue;
    public float currentValue { get; protected set; }
    public bool locked;

    public Timer(float _restartValue, float _currentValue) {
        restartValue = _restartValue;
        currentValue = _currentValue;
    }

    public abstract void UpdateTimer();

    public void ResetTimer()
    {
        currentValue = restartValue;
    }
}
