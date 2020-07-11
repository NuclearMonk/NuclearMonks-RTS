using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class State_MoveTo : IState
{
    UnitController _unitController;
    Vector3 _destination;
    public State_MoveTo(UnitController unitController)
    {
        _unitController = unitController;
    }
    public void OnEnter()
    {
        _destination = _unitController._targetLocation;
        _unitController._agent.SetDestination(_destination);
        _unitController._agent.avoidancePriority = Mathf.RoundToInt( Random.Range(50,100));

    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_destination != _unitController._targetLocation)
        {
            _destination = _unitController._targetLocation;
            _unitController._agent.SetDestination(_destination);
        }
        if (Vector3.SqrMagnitude(_unitController.transform.position-_destination)<0.1)
        {
            _unitController._isAtDestination = true;
        }
    }
}
