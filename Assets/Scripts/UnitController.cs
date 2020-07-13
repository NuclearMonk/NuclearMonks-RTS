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
    public NavMeshObstacle _obstacle;
    Outline[] _outlines;



    public bool _isSelected;
    StateMachine _stateMachine;
    public bool _isAtDestination = true;
    public Vector3 _destinationLocation;
    public bool hasTarget { get; set; } = false;


    public List<IAttackable> _inDetectionRange = new List<IAttackable>();
    public List<IAttackable> targets { get => _inDetectionRange; set => _inDetectionRange = value; }

    

    public int _team;
    public int team => _team;

    public List<IAttacker> _attackers { get; set; } = new List<IAttacker>();

    public int damage { get; } = 1;

    public int health { get; set; } = 10;
    public float range { get; } = 1f;
    public float rampuptime { get; } = 0.7f;
    public float cooldownTime { get; } = 2f;
    public bool isoncooldown { get; set; } = false;



    //public List<IAttackable> targets => _inDetectionRange;




    // Start is called before the first frame update
    private void Awake()
    {
        _outlines = GetComponentsInChildren<Outline>();
        _animationmotor = GetComponent<Animationmotor>();
        _agent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();
        _stateMachine = new StateMachine();

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
    Func<bool> HasTarget() => () => hasTarget;
    Func<bool> HasNoTarget() => () => !hasTarget;
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

    #region SelectionFunctions
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
    #endregion

    #region AttakerFunctions
    public void NewAttackableInDetectionRange(IAttackable attackable)
    {
        if (attackable.team != _team)
        {
            _inDetectionRange.Add(attackable);
            attackable._attackers.Add(this);
            hasTarget = true;
            //Debug.Log("New Attackable in Detection Range of " + gameObject.name, this);
        }

    }

    public void RemoveAttackableInDetectionRange(IAttackable attackable)
    {
        if (attackable.team != _team)
        {
            _inDetectionRange.Remove(attackable);
            attackable._attackers.Remove(this);
            //Debug.Log("Removed Attackable in Detection Range of" + gameObject.name, this);
        }


    }

    public void Attack(IAttackable target)
    {

        StartCoroutine(CooldownCoroutine());
        if (target != null)
        {
            target.TakeDamage(this, damage);
        }




    }
    public IEnumerator AttackCoroutine(IAttackable target)
    {
        _animationmotor.AttackAnimation();
        isoncooldown = true;
        if (target!=null)
        {
            yield return new WaitForSeconds(rampuptime);//Pobably should be cached
            Attack(target);
        }


    }
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime); //Pobably should be cached
        isoncooldown = false;
    }
    #endregion
    #region AttackableFunctions
    public void DisableThenDestroy()
    {
        DestructionRemoval();
        if (this != null)
        {
            gameObject.SetActive(false);
        }



    }

    public void TakeDamage(IAttacker attacker, int damage)
    {
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
    private void OnDisable()
    {
        Destroy(gameObject, 0.5f);
    }
    #endregion
}
