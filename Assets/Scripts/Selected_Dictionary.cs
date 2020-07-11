using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected_Dictionary : MonoBehaviour
{
    private Dictionary<int, GameObject> _selectedTable = new Dictionary<int, GameObject>();
    private Dictionary<int, ISelectable> _cachedSelectScripts = new Dictionary<int, ISelectable>();

    public void AddSelection(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(_selectedTable.ContainsKey(id)))
        {
            _selectedTable.Add(id, go);
            if (!_cachedSelectScripts.ContainsKey(id))
            {
                _cachedSelectScripts.Add(id, go.GetComponent<ISelectable>());
            }
            _cachedSelectScripts.TryGetValue(id, out ISelectable selectable);
            if (selectable != null)
            {

                selectable.Select();
            }

        }
    }
    public void RemoveSelection(GameObject go)
    {
        int id = go.GetInstanceID();
        _cachedSelectScripts.TryGetValue(id, out ISelectable selectable);
        if (selectable != null)
        {
            selectable.Deselect();
            _selectedTable.Remove(id);
        }

    }

    [ContextMenu("Remove Selections")]
    public void RemoveSelections()
    {
        foreach (int id in _selectedTable.Keys)
        {
            _selectedTable.TryGetValue(id, out GameObject toBeDeselected);
            _cachedSelectScripts.TryGetValue(id, out ISelectable selectable);
            if (toBeDeselected != null && selectable !=null)
            {
                selectable.Deselect();
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
    public ISelectable GameObjectSelectable(GameObject go)
    {
        ISelectable selectable;
        int id = go.GetInstanceID();
        _cachedSelectScripts.TryGetValue(id,out selectable);
        if (selectable != null)
        {
            return selectable;
        }
        else
        {
            selectable = go.GetComponent<ISelectable>();
            if (selectable != null)
            {
                _cachedSelectScripts.Add(id, selectable);
                return selectable;
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
