using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI for marking the place where the player shoots.
/// </summary>
public class ViewfinderUI : MonoBehaviour{

    [SerializeField]
    Canvas canvas;

    /// <summary>
    /// Places the ViewFinder at the mouse position.
    /// </summary>
    void Update(){
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }
}
