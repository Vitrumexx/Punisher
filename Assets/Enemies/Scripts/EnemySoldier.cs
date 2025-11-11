using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : MonoBehaviour
{
    
    [SerializeField] private StateController RagdollStates;
    [SerializeField] private RagdollHandler RagdollHandler;
    [SerializeField] private Transform raycastOrigin;

    public float rotationSpeed = 5f;
    public Transform[] patrolPoints;
    public Animator animator;
    public LayerMask layerMask;

    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Collider collision;
    private EnemyShoot shooting;
    private Transform player;
    private RaycastHit hit;

    public bool CanPatrol;
    public bool Dead;
    public bool seeingPlayer = false;

    void Start()
    {
        Dead = false;
        RagdollStates.Initialize();
        RagdollHandler.Initialize();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        shooting = GetComponent<EnemyShoot>();
        collision = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (!Dead)
            {
            animator.SetFloat("Speed", agent.speed);
            LookTarget(player);

            if (Input.GetKeyDown(KeyCode.P))
                CanPatrol = !CanPatrol;
            if (CanPatrol)
            {
                Patrol();
            }
            else return;
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

    public void StopSoldier()
    {
        agent.isStopped = true;
    }
    public void ResumeSoldier()
    {
        agent.isStopped = false;
    }

    void LookTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (Physics.Raycast(raycastOrigin.position, direction, out hit, shooting.maxVisionDistance * 2, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(raycastOrigin.position, direction * hit.distance * 2, Color.green);
                seeingPlayer = true;
            }
            else
            {
                Debug.DrawRay(raycastOrigin.position, direction * hit.distance * 2, Color.red);
                seeingPlayer = false;
            }
        }
        else return;
    }

    public void ChaseTarget(Transform target)
    {
        if (shooting.distanceToPlayer <= shooting.maxVisionDistance)
        {
            agent.isStopped = false;
            agent.speed = 6f;
            agent.SetDestination(target.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                CanPatrol = true;
                Patrol();
                return;
            }
        }

        if (shooting.distanceToPlayer <= shooting.attackDistance * 0.9f && seeingPlayer)
        {
            agent.isStopped = true;
            return;
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
            collision.enabled = false;
        }
    }
}
