using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [Tooltip("Value of the item")]
    [SerializeField] private int amount;

    public int Amount
    {
        get => amount;
    }

    public int Pick()
    {
        Destroy(gameObject);
        return Amount;
    }
}
