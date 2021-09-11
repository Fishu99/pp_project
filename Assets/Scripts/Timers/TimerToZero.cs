using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A timer that counts to zero.
/// </summary>
public class TimerToZero : Timer
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="_restartValue">the value set as currentValue when the timer is restarted</param>
    /// <param name="_currentValue">initial currentValue of the timer</param>
    public TimerToZero(float _restartValue, float _currentValue) :base(_restartValue, _currentValue) {
    }

    /// <summary>
    /// Reduces the currentValue by the time that has passed since last update.
    /// </summary>
    public override void UpdateTimer()
    {
        if (!locked && currentValue > 0)
            currentValue = currentValue - Time.fixedDeltaTime * 100f;    
    }
}
