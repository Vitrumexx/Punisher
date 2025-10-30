using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private int currentPatrolIndex;

    public Transform[] patrolPoints;
    public Animator animator;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.speed);
        Patrol();
    }

    void Patrol()
    {
        agent.speed = 3f;
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            GoToNextPatrolPoint();
        }


    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
}
