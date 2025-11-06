using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Время жизни в секундах после спавна")]
    public float lifetime = 1f;

    [Tooltip("Уничтожать при первом столкновении")]
    public bool destroyOnCollision = false;

    // Опционально: скорость для визуализации/логики (не используется для уничтожения)
    public float initialSpeed;
    [SerializeField]private float damage;

    private Collider ownerCollider;
    private Coroutine lifeCoroutine;

    private void OnEnable()
    {
        // Запускаем отсчёт жизни
        if (lifeCoroutine != null) StopCoroutine(lifeCoroutine);
        lifeCoroutine = StartCoroutine(DestroyAfterLifetime());
    }



    public void Initialize(float damageValue, Collider owner)
    {
        damage = damageValue;
        ownerCollider = owner;

        // Игнорируем коллизию с владельцем
        Collider projectileCollider = GetComponent<Collider>();
        if (projectileCollider != null && ownerCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, ownerCollider);
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Debug.Log($"[Projectile] Lifetime ended for {name}");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Projectile] Collided with {other.name}");

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            Debug.Log("[Projectile] Hit enemy!");
            enemy.TakeDamage(damage);
        }

        if (destroyOnCollision)
        {
            Debug.Log("[Projectile] Destroying due to collision");
            Destroy(gameObject);
        }
    }
}
