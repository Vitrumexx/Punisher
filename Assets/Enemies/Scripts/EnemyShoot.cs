using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Animator characterAnimator;
    [SerializeField] private DistantWeapon distantWeapon;
    public GameObject projectile;

    private AudioClip shotClip;
    private AudioSource audio;
    [SerializeField] private Transform projSpawnPoint;
    private Camera mainCamera;
    private Ray TargetRay;
    private Coroutine shootingCoroutine;
    private bool isAiming = false; // локальный флаг только для стрельбы от бедра
    private PlayerHandState handState;

    [SerializeField] private Collider ownerCollider;


    private Transform player;
    private Transform target;
    public float attackDistance = 10f;
    private Animator animator;


    private bool isShooting;
    private EnemySoldier soldier;
    void Start()
    {
        //distantWeapon = GetComponentInChildren<DistantWeapon>();
        ownerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        soldier = GetComponent<EnemySoldier>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        audio = GetComponentInChildren<AudioSource>();
        shotClip = audio.clip;
    }

    private void Update()
    {
        if (!soldier.Dead)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            animator.SetBool("isAiming", isShooting);
            if (distanceToPlayer <= attackDistance)
            {
                if (shootingCoroutine == null) shootingCoroutine = StartCoroutine(ShootingLoop());
                isShooting = true;
                soldier.CanPatrol = false;
                FaceTarget(player);
            }
            else
            {
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                    shootingCoroutine = null;
                }
                
                isShooting = false;
            }
        }
        else
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
            isShooting = false;
        }
    }
    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private IEnumerator ShootingLoop()
    {
        float interval = 1f / distantWeapon.shootingSpeed;
        while (true)
        {
            int shots = Random.Range(4, 10);
            for (int i = 0; i < shots; i++)
            {
                Shoot(player);
                yield return new WaitForSeconds(interval);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void Shoot(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        TargetRay = new Ray(projSpawnPoint.position, direction);
        if (projectile == null || projSpawnPoint == null) return;
        audio.PlayOneShot(audio.clip);

        GameObject proj = Instantiate(projectile, projSpawnPoint.position, projSpawnPoint.rotation);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.Initialize(distantWeapon.damage, ownerCollider);

        if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(TargetRay.direction * 300f, ForceMode.Impulse);
        }
    }

    void MoveToNextPosition()
    {

    }

   
}

    
