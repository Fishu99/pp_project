using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for timers.
/// </summary>
public abstract class Timer
{
    /// <summary>
    /// The value set as currentValue when the timer is restarted.
    /// </summary>
    protected float restartValue;

    /// <summary>
    /// Current value of the timer.
    /// </summary>
    public float currentValue { get; protected set; }

    /// <summary>
    /// Tells if a timer is locked.
    /// </summary>
    public bool locked;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="_restartValue">the value set as currentValue when the timer is restarted</param>
    /// <param name="_currentValue">initial currentValue of the timer</param>
    public Timer(float _restartValue, float _currentValue) {
        restartValue = _restartValue;
        currentValue = _currentValue;
    }

    /// <summary>
    /// Method called regularly in FixedUpdate.
    /// </summary>
    public abstract void UpdateTimer();

    /// <summary>
    /// Resets a timer. After the reset the currentValue is equal to restartValue.
    /// </summary>
    public void ResetTimer()
    {
        currentValue = restartValue;
    }
}
