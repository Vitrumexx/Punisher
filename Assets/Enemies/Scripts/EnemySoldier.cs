using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : MonoBehaviour
{
    public bool Dead;
    [SerializeField] private StateController RagdollStates;
    [SerializeField] private RagdollHandler RagdollHandler;


    public float rotationSpeed = 5f;
    private int currentPatrolIndex;

    public Transform[] patrolPoints;
    public Animator animator;
    private NavMeshAgent agent;
    private bool CanPatrol;
    void Start()
    {
        Dead = false;
        RagdollStates.Initialize();
        RagdollHandler.Initialize();

        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (!Dead)
            {
            animator.SetFloat("Speed", agent.speed);
            if (Input.GetKeyDown(KeyCode.P))
                CanPatrol = !CanPatrol;
            if (CanPatrol)
            {
                Patrol();
            }
            else
                agent.speed = 0;
        }
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

    public void Death()
    {
        if (!Dead)
        {
            CanPatrol = false;
            agent.enabled = false;
            RagdollStates.DisableAnimator();
            RagdollHandler.Enable();
            Debug.Log(gameObject.name + " dead");
            Dead = true;
        }
    }
}
