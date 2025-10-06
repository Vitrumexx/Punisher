using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mutant_Melee_Attack : MonoBehaviour
{
    private Parameters parameters;
    private NavMeshAgent agent;
    [SerializeField] private Collider[] attackColliders;
    private Animator anim;
    void Start()
    {
        for (int i = 0; i < attackColliders.Length; i++)
            attackColliders[i].enabled = false;

        agent = GetComponent<NavMeshAgent>();
        parameters = GetComponent<Parameters>();
        anim = GetComponent<Animator>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.SetDestination(other.transform.position);
        }
    }
    void MeleeHit()
    {
        for (int i = 0; i < attackColliders.Length; i++)
            attackColliders[i].enabled = true;
        anim.SetTrigger("DefaultMeleeAttack");
    }
}
