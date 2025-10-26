using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponLogic : MonoBehaviour
{
    public Animator characterAnimator;
    private MeleeWeapon meleeWeaponData;

    public void Init(Animator animator, MeleeWeapon data)
    {
        characterAnimator = animator;
        meleeWeaponData = data;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        ApplyAnimation();
    }

    private void OnDestroy()
    {
        // сброс анимации
        ResetAnimator();
    }

    private void ApplyAnimation()
    {
        if (characterAnimator == null || meleeWeaponData == null) return;

        ResetAnimator();
        switch (meleeWeaponData.WeaponType)
        {
            case MeleeWeaponType.Spear:
                characterAnimator.SetInteger("MeleeWeaponType", 1);
                break;
            case MeleeWeaponType.Sword:
                characterAnimator.SetInteger("MeleeWeaponType", 2);
                break;
        }
    }

    public void ResetAnimator()
    {
        if (characterAnimator == null) return;
        characterAnimator.SetInteger("MeleeWeaponType", 0);
        // isAiming сбрасывать не будем здесь
    }

    private void Update()
    {
        Attack();
    }
    void Attack()
    {
        if (characterAnimator == null || meleeWeaponData == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attack");
            characterAnimator.SetTrigger("Attack");
        }
    }
    
    
}

