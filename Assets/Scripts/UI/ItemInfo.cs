using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfo : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Camera mainCamera;

    CanvasGroup canvasGroup;

    TextMeshProUGUI textMesh;

    Vector3 currentPosition;
    Vector2 pos;
    Vector2 screenPos;
    
    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        screenPos = mainCamera.WorldToScreenPoint(currentPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
        
    }

    public void Show(string name, Vector3 newPosition){
        textMesh.text = name;
        currentPosition = newPosition;
        canvasGroup.alpha = 1;
    }

    public void Hide(){
        canvasGroup.alpha = 0;
        textMesh.text = "";
    }

}
