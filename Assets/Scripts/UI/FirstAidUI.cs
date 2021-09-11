using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI for displaying the number of available first aid kits.
/// </summary>
public class FirstAidUI : MonoBehaviour{

    /// <summary>
    /// Text displaying the number of available first aid kits.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI text;

    /// <summary>
    /// AudioSource for playing sounds.
    /// </summary>
    [SerializeField]
    AudioSource audioSource;

    /// <summary>
    /// A sound played when the number of first aid kits is increased.
    /// </summary>
    [SerializeField]
    AudioClip getSound;

    /// <summary>
    /// A sound played when the number of first aid kits is decreased.
    /// </summary>
    [SerializeField]
    AudioClip useSound;

    /// <summary>
    /// Currently displayed number of first aid kits.
    /// </summary>
    int currentNumer;

    /// <summary>
    /// Initializes the number of kits to 0.
    /// </summary>
    void Awake(){
        text.text = "0";
        currentNumer = 0;
    }

    /// <summary>
    /// Sets a new number of first aid kits.
    /// </summary>
    /// <param name="n">new number of first aid kits.</param>
    public void SetNumbers(int n){
        if(currentNumer > n){
            audioSource.PlayOneShot(useSound);
        }else if(currentNumer < n){
            audioSource.PlayOneShot(getSound);
        }
        currentNumer = n;
        text.text = n.ToString();
    }


}
