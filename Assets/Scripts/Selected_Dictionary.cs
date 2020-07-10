using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected_Dictionary : MonoBehaviour
{
    private Dictionary<int, GameObject> _selectedTable = new Dictionary<int, GameObject>();
    private Dictionary<int, SelectScript> _cachedSelectScripts = new Dictionary<int, SelectScript>();

    public void AddSelection(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(_selectedTable.ContainsKey(id)))
        {
            _selectedTable.Add(id, go);
            if (!_cachedSelectScripts.ContainsKey(id))
            {
                _cachedSelectScripts.Add(id, go.GetComponent<SelectScript>());
            }
            _cachedSelectScripts.TryGetValue(id, out SelectScript selected);
            if (selected != null)
            {

                selected.enabled = true;
            }

        }
    }
    public void RemoveSelection(GameObject go)
    {
        int id = go.GetInstanceID();
        _cachedSelectScripts.TryGetValue(id, out SelectScript selected);
        if (selected != null)
        {
            selected.enabled = false;
            _selectedTable.Remove(id);
        }

    }

    [ContextMenu("Remove Selections")]
    public void RemoveSelections()
    {
        foreach (int id in _selectedTable.Keys)
        {
            _selectedTable.TryGetValue(id, out GameObject toBeDeselected);
            _cachedSelectScripts.TryGetValue(id, out SelectScript selected);
            if (toBeDeselected != null && selected !=null)
            {
                selected.enabled = false;
            }
        }
        _selectedTable.Clear();
    }

    public void ToggleSelection(GameObject go)
    {
        if (go == null) return;
        int id = go.GetInstanceID();
        Debug.Log(go.name,this);
        if (!(_selectedTable.ContainsKey(id)))
        {
            AddSelection(go);
        }
        else
        {
            RemoveSelection(go);
        }
    }
    public SelectScript GameObjectSelectable(GameObject go)
    {
        SelectScript selected;
        int id = go.GetInstanceID();
        _cachedSelectScripts.TryGetValue(id,out selected);
        if (selected != null)
        {
            return selected;
        }
        else
        {
            selected = go.GetComponent<SelectScript>();
            if (selected != null)
            {
                _cachedSelectScripts.Add(id, selected);
                return selected;
            }
        }
        return null;
    }
    public bool HasUnitsSelected()
    {
        foreach(GameObject go in _selectedTable.Values)
        {
            if(go != null)
            {
                return true;
            }
        }
        return false;
    }
    public List<UnitController> SelectedUnitControlers()
    {
        List<UnitController> returnedList = new List<UnitController>();
        UnitController unitController;
        foreach (GameObject go in _selectedTable.Values)
        {
            if (go != null)
            {
                unitController = go.GetComponent<UnitController>();
                if (unitController != null)
                {
                    returnedList.Add(unitController);
                }
            }
           
        }
        return returnedList;
    }
}
