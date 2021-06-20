using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstAidUI : MonoBehaviour{

    [SerializeField]
    TextMeshProUGUI text;

    void Awake(){
        text.text = "0";
    }

    public void SetNumbers(int n){
        text.text = n.ToString();
    }


}
