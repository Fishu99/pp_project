using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] public Dictionary<string, Timer> timers;

    void Start()
    {
        timers = new Dictionary<string, Timer>();
    }

    void FixedUpdate()
    {
        foreach( KeyValuePair<string, Timer> kvp in timers) {
            kvp.Value.UpdateTimer();
        }
    }

    public void AddTimer(string id, Timer timer) {
        timers.Add(id, timer);
    }

    public bool IsTimerLocked(string id) {
        bool f = true;
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) f = timer.locked;
        return f;
    }

    public float GetStatusOfTimer(string id) {
        float f = 0f;
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) f = timer.currentValue;
        return f;
    }

    public void ResetTimer(string id) {
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) timer.ResetTimer();
    }

    public void SetLock(string id, bool lockStatus) {
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) timer.locked = lockStatus;
    }
}
