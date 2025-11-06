using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeaponTrigger : MonoBehaviour
{
    private MeleeWeapon meleeWeaponData;
    private EnemyHealth enemyHealth;

    public void Init(MeleeWeapon data)
    {
         meleeWeaponData = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(meleeWeaponData.damage);
        }
    }
}
