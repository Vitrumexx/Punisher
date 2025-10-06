using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaLogic : MonoBehaviour
{
    public PlayerController controller;
    public PlayerParameters playerParam;

    void Update()
    {
        Sprint();
        StaminaRegeneration();
        Jump();
    }
    void Sprint()
    {
        if (controller.isSprinting == true && playerParam._stamina > 0)
            playerParam._stamina -= Time.deltaTime * 5;
    }

    void StaminaRegeneration()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && playerParam._stamina < playerParam._maxStamina && controller.isGrounded)
            playerParam._stamina += Time.deltaTime * 10;
        if (playerParam._stamina > playerParam._maxStamina)
            playerParam._stamina = playerParam._maxStamina;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && controller.isGrounded && playerParam._stamina > 10)
            playerParam._stamina -= 20;
    }
}
