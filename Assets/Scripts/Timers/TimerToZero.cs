using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerToZero : Timer
{   
    public TimerToZero(float _restartValue, float _currentValue) :base(_restartValue, _currentValue) {
    }

    public override void UpdateTimer()
    {
        if (!locked && currentValue > 0)
            currentValue = currentValue - Time.fixedDeltaTime * 100f;    
    }
}
