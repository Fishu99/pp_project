using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for ammunition box.
/// </summary>
public class Ammunition : MonoBehaviour
{
    /// <summary>
    /// Amount of ammunition in the box.
    /// </summary>
    [Tooltip("Value of the item")]
    [SerializeField] private int amount;

    /// <summary>
    /// Gets the amount of ammunition in the box.
    /// </summary>
    public int Amount
    {
        get => amount;
    }

    /// <summary>
    /// Picks the ammunition.
    /// </summary>
    /// <returns>the amount of ammunition in the box.</returns>
    public int Pick()
    {
        Destroy(gameObject);
        return Amount;
    }
}
