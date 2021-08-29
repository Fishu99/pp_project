using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour{

    [SerializeField]
    RectTransform objectToRotate;

    [SerializeField]
    RectTransform toShow;

    [SerializeField]
    float speed = 2f;

    Vector3 rotationObject;

    void Start(){
        toShow.gameObject.SetActive(false);
        LevelsController.Instance.BeforeChangeLevel += () => { toShow.gameObject.SetActive(true); };
        LevelsController.Instance.AfterChangeLevel += () => { toShow.gameObject.SetActive(false); };
    }

    void Update(){
        rotationObject += new Vector3(0,0,Time.unscaledDeltaTime * 50f * speed);
        rotationObject.z = rotationObject.z > 360 ? rotationObject.z - 360 : rotationObject.z;
        objectToRotate.rotation = Quaternion.Euler(rotationObject);
    }
}
