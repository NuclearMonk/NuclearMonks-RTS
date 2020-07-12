using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class State_Attacking : IState
{
    IAttacker _attacker;
    UnitController _unitController;
    public State_Attacking(IAttacker attacker,UnitController unitController)
    {
        _attacker = attacker;
        _unitController = unitController;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {

        if (_attacker.targets.Count == 0)
        {
            _attacker._hasTarget = false;
        }
        else
        {
            IAttackable target = _attacker.targets.OrderBy(possibletarget => Vector3.SqrMagnitude(possibletarget.transform.position - _attacker.transform.position)).First();
            if (Vector3.Distance(target.transform.position, _attacker.transform.position) > _attacker._range)
            {
                _unitController._agent.SetDestination(target.transform.position + Vector3.Normalize(_attacker.transform.position- target.transform.position) * (_attacker._range-0.1f));
            }
            else
            {
                _attacker.Attack(target);
            }
        }
       
        
    }
}
