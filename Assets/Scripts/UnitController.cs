using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour, ISelectable,IAttacker
{

    public NavMeshAgent _agent;
    Outline[] _outlines;


    
    public bool _isSelected;
    StateMachine _stateMachine;
    public bool _isAtDestination = true;
    public Vector3 _targetLocation;
    private List<IAttackable> _inDetectionRange = new List<IAttackable>();

    

    // Start is called before the first frame update
    private void Awake()
    {
        _outlines = GetComponentsInChildren<Outline>();
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();

    }

    void Start()
    {
        var idle = new State_Idle();
        var moveTo = new State_MoveTo(this);
        _stateMachine.AddTransition(idle, moveTo, IsNotAtDestination());
        _stateMachine.AddTransition(moveTo,idle,  IsAtDestination());
        _stateMachine.SetState(idle);
    }
    Func<bool> IsNotAtDestination() => () => !_isAtDestination;
    Func<bool> IsAtDestination() => () => _isAtDestination;

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
    }
    public void UpdateDestination(Vector3 newDestination)
    {
        _targetLocation = newDestination;
        _isAtDestination = false;
    }

    public void Select()
    {
        _isSelected = true;
        foreach (Outline outline in _outlines)
        {
            outline.enabled = true;
        }
    }

    public void Deselect()
    {
        _isSelected = false;
        foreach (Outline outline in _outlines)
        {
            outline.enabled = false;
        }
    }

    public void NewAttackableInDetectionRange(IAttackable attackable)
    {
        _inDetectionRange.Add(attackable);
        Debug.Log("New Attackable in Detection Range of" + gameObject.name, this);
    }

    public void RemoveAttackableInDetectionRange(IAttackable attackable)
    {
        _inDetectionRange.Remove(attackable);
        Debug.Log("Removed Attackable in Detection Range of" + gameObject.name, this);
    }

    public float CheckRange(IAttackable attackable)
    {
        throw new NotImplementedException();
    }

    public void Attack(IAttackable target)
    {
        throw new NotImplementedException();
    }
}
