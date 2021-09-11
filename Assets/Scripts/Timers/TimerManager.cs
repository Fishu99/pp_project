using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for managing timers.
/// </summary>
public class TimerManager : MonoBehaviour
{   
    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

    void Start()
    {
    }

    /// <summary>
    /// Updates all the timesr.
    /// </summary>
    void FixedUpdate()
    {
        foreach( KeyValuePair<string, Timer> kvp in timers) {
            kvp.Value.UpdateTimer();
        }
    }

    /// <summary>
    /// Adds a timer to the manager.
    /// </summary>
    /// <param name="id">identifier of the timer</param>
    /// <param name="timer">the timer to add</param>
    public void AddTimer(string id, Timer timer) {
        timers.Add(id, timer);
    }

    /// <summary>
    /// Returns a value indicating if a timer is locked.
    /// </summary>
    /// <param name="id">identifier of the timer</param>
    /// <returns>true if the timer is locked</returns>
    public bool IsTimerLocked(string id) {
        bool f = true;
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) f = timer.locked;
        return f;
    }

    /// <summary>
    /// Returns the current value of the timer.
    /// </summary>
    /// <param name="id">identifier of the timer</param>
    /// <returns>the current value of the timer</returns>
    public float GetStatusOfTimer(string id) {
        float f = 0f;
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) f = timer.currentValue;
        return f;
    }

    /// <summary>
    /// Resets a timer.
    /// </summary>
    /// <param name="id">identifier of the timer</param>
    public void ResetTimer(string id) {
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) timer.ResetTimer();
    }

    /// <summary>
    /// Locks or unlocks a timer.
    /// </summary>
    /// <param name="id">identifier of the timer</param>
    /// <param name="lockStatus">if true, the timer will be unlocked, if false, the timer will be unlocked.</param>
    public void SetLock(string id, bool lockStatus) {
        Timer timer = null;
        timers.TryGetValue(id, out timer);
        if(timer != null) timer.locked = lockStatus;
    }
}
