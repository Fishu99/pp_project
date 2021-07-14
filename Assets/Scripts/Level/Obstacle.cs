using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int obstacleType;

    public int getType() {
        return obstacleType;
    }
}
