using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float _maxHealth;
    public float _health;
    

    private EnemySoldier enemyScript;
    void Start()
    {
        
        _health = _maxHealth;
        enemyScript = GetComponent<EnemySoldier>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            enemyScript.Death();
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("TakeDamage triggered");
        _health -= damage;
        // Можно добавить лёгкий клип эффект/анимацию попадания
        // enemyScript.PlayHitAnimation();

        if (_health <= 0 && !enemyScript.Dead)
        {
            enemyScript.Death();
        }
    }
}
