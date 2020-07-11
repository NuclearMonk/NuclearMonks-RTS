using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animationmotor : MonoBehaviour
{

    Animator _animator;
    NavMeshAgent _agent;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("animationSpeed", _agent.velocity.magnitude / _agent.speed);
    }
    [ContextMenu("Punch")]
    public void punch()
    {
        _animator.SetTrigger("Attack");
    }
}
