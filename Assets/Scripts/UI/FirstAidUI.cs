using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstAidUI : MonoBehaviour{

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip getSound;

    [SerializeField]
    AudioClip useSound;

    int currentNumer;

    void Awake(){
        text.text = "0";
        currentNumer = 0;
    }

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
