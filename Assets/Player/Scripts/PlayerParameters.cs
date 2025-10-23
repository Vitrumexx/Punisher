using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public float _maxHealth;
    public float _health;
    public float _maxStamina;
    public float _stamina;
    public int _money;

    [Header("Ammo")]
    public int Pistol;
    public int AR;
    void Start()
    {
        _money = 0;
        _maxHealth = 100;
        _health = _maxHealth;
        _maxStamina = 100;
        _stamina = _maxStamina;
    }
}
