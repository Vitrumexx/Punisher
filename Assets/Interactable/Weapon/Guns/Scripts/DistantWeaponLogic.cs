using System.Collections;
using UnityEngine;

public class DistantWeaponLogic : MonoBehaviour
{
    private Animator characterAnimator;
    private DistantWeapon distantWeapon;
    public GameObject projectile;
    private AudioSource audio;
    [SerializeField] private PlayerParameters playerParam;
    [SerializeField] private Transform projSpawnPoint;
    private Camera mainCamera;
    private Ray TargetRay;
    private Coroutine shootingCoroutine;
    private bool isAiming = false; // локальный флаг только для стрельбы от бедра
    private PlayerHandState handState;

    public void Init(Animator animator, DistantWeapon weaponData)
    {
        audio = GetComponent<AudioSource>();
        characterAnimator = animator;
        distantWeapon = weaponData;
        mainCamera = Camera.main;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            handState = player.GetComponent<PlayerHandState>();
        playerParam = handState.gameObject.GetComponent<PlayerParameters>();
        ApplyAnimation();
    }

    private void OnDestroy()
    {
        // сброс анимации
        ResetAnimator();
    }

    private void ApplyAnimation()
    {
        if (characterAnimator == null || distantWeapon == null) return;

        ResetAnimator();
        if (handState != null)
        {
            handState.CarryPistol = false;
            handState.CarryRifle = false;
        }

        switch (distantWeapon.WeaponType)
        {
            case DistantWeaponType.Pistol:
                if (handState != null) handState.CarryPistol = true;
                characterAnimator.SetBool("CarryRifle", handState.CarryRifle);
                characterAnimator.SetBool("CarryPistol", handState.CarryPistol);
                break;

            case DistantWeaponType.AssaultRifle:
                if (handState != null) handState.CarryRifle = true;
                characterAnimator.SetBool("CarryPistol", handState.CarryPistol);
                characterAnimator.SetBool("CarryRifle", handState.CarryRifle);
                break;
        }
    }

    public void ResetAnimator()
    {
        if (characterAnimator == null) return;
        characterAnimator.SetBool("CarryPistol", false);
        characterAnimator.SetBool("CarryRifle", false);
        // isAiming сбрасывать не будем здесь
    }

    private void Update()
    {
        if (characterAnimator == null || distantWeapon == null) return;
        
        // Проверяем, зажата ли ПКМ — если да, прицеливание управляется CameraRotation
        bool isRightMouseHeld = Input.GetKey(KeyCode.Mouse1);

        // Только если ПКМ не зажата, DistantWeaponLogic может менять локальный isAiming
        if (!isRightMouseHeld)
        {
            HandleHipFireAiming();
        }
            
        // Стрельба
        HandleShooting(isRightMouseHeld);
    }

    private void HandleHipFireAiming()
    {
        // подъем оружия при стрельбе от бедра
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAiming)
        {
            characterAnimator.SetBool("isAiming", true);
            isAiming = true;
        }
        ApplyAnimation();
    }

    private void HandleShooting(bool isRightMouseHeld)
    {
        if (distantWeapon == null) return;
        
        switch (distantWeapon.WeaponType)
        {
            case DistantWeaponType.Pistol:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    FireProjectile();
                break;

            case DistantWeaponType.AssaultRifle:
                if (Input.GetKey(KeyCode.Mouse0) && shootingCoroutine == null)
                    shootingCoroutine = StartCoroutine(ShootingLoop());

                if (!Input.GetKey(KeyCode.Mouse0) && shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                    shootingCoroutine = null;
                    // сбрасываем локальный isAiming только если стрельба от бедра
                    if (!Input.GetKey(KeyCode.Mouse1))
                    {
                        characterAnimator.SetBool("isAiming", false);
                        isAiming = false;
                    }
                }
                break;
        }
    }

    private IEnumerator ShootingLoop()
    {
        float interval = 1f / distantWeapon.shootingSpeed;

        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(interval);
        }
    }

    private void FireProjectile()
    {
        if (playerParam._bullets > 0)
        {
            TargetRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            if (projectile == null || projSpawnPoint == null) return;
            audio.PlayOneShot(audio.clip);
            GameObject proj = Instantiate(projectile, projSpawnPoint.position, projSpawnPoint.rotation);
            if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.AddForce(TargetRay.direction * 300f, ForceMode.Impulse);
            }
            playerParam._bullets--;
        }
        else
            return;
    }
}