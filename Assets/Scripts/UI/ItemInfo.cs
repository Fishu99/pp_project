using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI for presenting information about an item which can be picked up by the player.
/// </summary>
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
    
    /// <summary>
    /// Initializes the components.
    /// </summary>
    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Updates the position of the item description.
    /// </summary>
    void Update()
    {
        screenPos = mainCamera.WorldToScreenPoint(currentPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
        
    }

    /// <summary>
    /// Shows a string describing an item.
    /// </summary>
    /// <param name="name">The name of the item which will be displayed.</param>
    /// <param name="newPosition">The position of the item.</param>
    public void Show(string name, Vector3 newPosition){
        textMesh.text = name;
        currentPosition = newPosition;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Hides a text describing an item.
    /// </summary>
    public void Hide(){
        canvasGroup.alpha = 0;
        textMesh.text = "";
    }

}
