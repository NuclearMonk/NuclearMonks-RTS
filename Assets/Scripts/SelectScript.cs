using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScript : MonoBehaviour
{
    public UnitController unitControler;

    private void Awake()
    {
        unitControler = GetComponent<UnitController>();
    }
    private void OnEnable()
    {
        OnSelect();
    }

    private void OnSelect()
    {
        unitControler.isSelected = true;
        var outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = true;
        }
    }
    private void OnDisable()
    {
        unitControler.isSelected = false;
        var outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }
}
