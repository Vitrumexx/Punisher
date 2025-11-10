using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerParameters parameters;
    void Start()
    {
        parameters = GetComponent<PlayerParameters>();
    }

    private void FixedUpdate()
    {
        if (parameters._health > parameters._maxHealth)
            parameters._health = parameters._maxHealth;
    }
    public void Death()
    {

    }

    public void TakeDamage(float damage)
    {
        parameters._health -= damage;
    }
}
