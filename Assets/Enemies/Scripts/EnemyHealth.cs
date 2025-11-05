using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    private EnemySoldier enemyScript;
    void Start()
    {
        enemyScript = GetComponent<EnemySoldier>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            enemyScript.Death();
        }
        /*
        if (param.health <= 0 && !Dead)
        {
            Death();
        }
        */
    }
}
