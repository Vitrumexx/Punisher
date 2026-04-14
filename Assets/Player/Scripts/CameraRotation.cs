using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour
{
    public Transform target;
    public Vector3 normalOffset = new Vector3(0, 2, -4);
    public Vector3 aimOffset = new Vector3(0.5f, 1.8f, -2f);
    public float sensitivity = 3f;
    public float distance = 4f;
    public float minY = -30f;
    public float maxY = 60f;
    public float smoothTime = 0.1f;
    public Animator playerAnim;
    public Image crosshair;

    private float yaw = 0f;
    private float pitch = 0f;

    private Vector3 currentVelocity;
    private Vector3 currentOffset;

    public bool isAiming = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        currentOffset = normalOffset;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minY, maxY);
        SetAiming();
        Vector3 targetOffset = isAiming ? aimOffset : normalOffset;
        currentOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref currentVelocity, smoothTime);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * currentOffset;

        transform.position = desiredPosition;

        
        if (isAiming)
        {
            transform.rotation = rotation;
        }
        else
        {
            transform.LookAt(target);
        }
    }

    public void SetAiming()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isAiming)
            {
                playerAnim.SetBool("isAiming", true);
                isAiming = true;
                crosshair.enabled = true;
            }
        }

        // Отпускание ЛКМ — останавливаем корутину
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (isAiming)
            {
                playerAnim.SetBool("isAiming", false);
                isAiming = false;
                crosshair.enabled = false;
            }
        }


    }

    public Vector3 GetLookDirection()
    {
        return transform.forward;
    }
}