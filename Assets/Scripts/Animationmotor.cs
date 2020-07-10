using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animationmotor : MonoBehaviour
{

    Animator animator;
    NavMeshAgent agent;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("animationSpeed", agent.velocity.magnitude / agent.speed);
    }
}
