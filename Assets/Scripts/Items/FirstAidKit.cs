using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for a first aid kit.
/// </summary>
public class FirstAidKit : MonoBehaviour
{

    /// <summary>
    /// Picks the first aid kit.
    /// </summary>
    public void Pick()
    {
        Destroy(gameObject);
    }
}
