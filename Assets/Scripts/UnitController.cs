using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool isSelected;
    StateMachine stateMachine;
    public bool isAtDestination = true;
    public Vector3 targetLocation;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine();
        var idle = new State_Idle();
        var moveTo = new State_MoveTo(this);

        stateMachine.AddTransition(idle, moveTo, IsNotAtDestination());
        stateMachine.AddTransition(moveTo,idle,  IsAtDestination());
        stateMachine.SetState(idle);
    }
    Func<bool> IsNotAtDestination() => () => !isAtDestination;
    Func<bool> IsAtDestination() => () => isAtDestination;

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();
    }
    public void UpdateDestination(Vector3 newDestination)
    {
        targetLocation = newDestination;
        isAtDestination = false;
    }
}
