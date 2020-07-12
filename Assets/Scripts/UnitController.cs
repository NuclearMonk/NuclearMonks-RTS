using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitController : MonoBehaviour, ISelectable, IAttacker, IAttackable
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

    public float _range { get; } = 1f;

    public int _team;
    public int team => _team;

    public List<IAttacker> _attackers { get; set; } = new List<IAttacker>();

    public int damage { get; } = 1;

    public int health { get; set; } = 10;

    public float cooldownTime { get; } = 1f;


    public bool cooldown { get; set; } = true;



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
        var attacking = new State_Attacking(this, this);
        _stateMachine.AddAnyTransition(moveTo, IsNotAtDestination());
        _stateMachine.AddTransition(moveTo, idle, IsAtDestination());
        _stateMachine.AddTransition(idle, attacking, HasTarget());
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
        if (attackable.team != _team)
        {
            _inDetectionRange.Add(attackable);
            attackable._attackers.Add(this);
            _hasTarget = true;
            Debug.Log("New Attackable in Detection Range of " + gameObject.name, this);
        }

    }

    public void RemoveAttackableInDetectionRange(IAttackable attackable)
    {
        if (attackable.team != _team)
        {
            _inDetectionRange.Remove(attackable);
            attackable._attackers.Remove(this);
            Debug.Log("Removed Attackable in Detection Range of" + gameObject.name, this);
        }


    }

    public float CheckRange(IAttackable attackable)
    {
        throw new NotImplementedException();
    }

    public void Attack(IAttackable target)
    {
        target.TakeDamage(this, damage);
        cooldown = false;
        StartCoroutine(CooldownCoroutine());
        _animationmotor.punch();
    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime); //Pobably should be cached
        cooldown = true;
    }

    public void DisableThenDestroy()
    {
        DestructionRemoval();
        Destroy(gameObject, 0.2f);
        gameObject.SetActive(false);

    }

    public void TakeDamage(IAttacker attacker, int damage)
    {
        Debug.Log(gameObject.name + " Was Attacked");
        health -= damage;
        if (health <= 0)
        {
            DisableThenDestroy();
        }
    }
    public void DestructionRemoval()
    {
        foreach (IAttacker attacker in _attackers)
        {
            attacker.targets.Remove(this);
        }
    }
}
