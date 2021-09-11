using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A utility class for disabling an object when the game is started.
/// </summary>
public class DisableInPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

}
