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
        Debug.Log("Batata");
        _unitController._obstacle.enabled = false;
        _unitController._agent.enabled = true;
        _destination = _unitController._destinationLocation;
        _unitController._agent.SetDestination(_destination+=new Vector3(Random.Range(-1,1),0,Random.Range(-1,1)));
        _unitController._agent.avoidancePriority = Mathf.RoundToInt(Random.Range(50, 100));
        Debug.Log(Mathf.RoundToInt(Random.Range(50, 100)));

    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_destination != _unitController._destinationLocation)
        {
            _destination = _unitController._destinationLocation;
            _unitController._agent.SetDestination(_destination);

        }
        if (Vector3.SqrMagnitude(_unitController.transform.position-_destination)<1)
        {
            _unitController._agent.enabled = false;
            _unitController._obstacle.enabled = true;
            _unitController._isAtDestination = true;
        }
    }
}
