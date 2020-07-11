using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class State_Attacking : IState
{
    IAttacker _attacker;
    
    public State_Attacking(IAttacker attacker)
    {
        _attacker = attacker;
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
            _attacker.targets.First().TakeDamage(_attacker, 1);
            Debug.Log("Batata");
        }
       
        
    }
}
