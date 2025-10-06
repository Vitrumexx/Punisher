using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Время жизни в секундах после спавна")]
    public float lifetime = 1f;

    [Tooltip("Уничтожать при первом столкновении")]
    public bool destroyOnCollision = true;

    // Опционально: скорость для визуализации/логики (не используется для уничтожения)
    public float initialSpeed;

    private Coroutine lifeCoroutine;

    private void OnEnable()
    {
        // Запускаем отсчёт жизни
        if (lifeCoroutine != null) StopCoroutine(lifeCoroutine);
        lifeCoroutine = StartCoroutine(DestroyAfterLifetime());
    }

    private void OnDisable()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!destroyOnCollision) return;

        // Можно здесь добавить эффекты попадания, урон и т.д.
        Destroy(gameObject);
    }
}
