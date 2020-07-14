using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildGrid))]
public class BuildingManager : MonoBehaviour
{
    [SerializeField] BuildGrid _buidgrid;
     public GameObject _currentlyBuilding = null;
    public void CreateBuildingBuildSketch(GameObject building)
    {
        _currentlyBuilding = GameObject.Instantiate(building, new Vector3(0, 100000f, 0), Quaternion.identity);
    }
    public void UpdateBuildingPosition(Vector3 position)
    {
        if (_currentlyBuilding == null) return;
        _currentlyBuilding.transform.position = _buidgrid.GetNearestPointOnGrid(position);
    }
}
