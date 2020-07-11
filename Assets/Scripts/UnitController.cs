using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour, ISelectable, IAttacker
{
    private Animationmotor _animationmotor;

    public NavMeshAgent _agent;
    Outline[] _outlines;



    public bool _isSelected;
    StateMachine _stateMachine;
    public bool _isAtDestination = true;
    public Vector3 _destinationLocation;
    public bool _hasTarget { get; set; } = false;
    

    public List<IAttackable> _inDetectionRange = new List<IAttackable>();
    public List<IAttackable> targets { get => _inDetectionRange; set => _inDetectionRange = value; }
    //public List<IAttackable> targets => _inDetectionRange;




    // Start is called before the first frame update
    private void Awake()
    {
        _outlines = GetComponentsInChildren<Outline>();
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _animationmotor = GetComponent<Animationmotor>();

    }

    void Start()
    {
        var idle = new State_Idle();
        var moveTo = new State_MoveTo(this);
        var attacking = new State_Attacking(this);
        _stateMachine.AddTransition(idle, moveTo, IsNotAtDestination());
        _stateMachine.AddTransition(moveTo,idle,  IsAtDestination());
        _stateMachine.AddAnyTransition(attacking,HasTarget());
        _stateMachine.AddTransition(attacking, idle, HasNoTarget());
        _stateMachine.SetState(idle);
    }
    Func<bool> IsNotAtDestination() => () => !_isAtDestination;
    Func<bool> IsAtDestination() => () => _isAtDestination;
    Func<bool> HasTarget() => () => _hasTarget;
    Func<bool> HasNoTarget() => () => !_hasTarget;
    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
    }
    public void UpdateDestination(Vector3 newDestination)
    {
        _destinationLocation = newDestination;
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
        _hasTarget = true;
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
        target.TakeDamage(this, 1);
        _animationmotor.punch();
    }

}
