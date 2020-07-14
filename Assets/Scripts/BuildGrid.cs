using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    public float size = 1f;
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        Vector3 newGridPosition = new Vector3(
             (float)size*Mathf.RoundToInt(position.x / size)
            ,(float)size * Mathf.RoundToInt(position.y / size)
            , (float)size * Mathf.RoundToInt(position.z / size));
        return newGridPosition;
    }
}
