using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameters playerParam;
    public Transform spineBone;
    public float spineRotateSpeed = 10f;
    public float maxSpineYaw = 60f;


    public float maxSpeed = 15f;
    public float walkSpeed = 3f;
    public float standardSpeed = 8f;
    public float currentSpeed;
    public float speed;

    public float rotationSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public Animator animator;
    public Transform cameraTransform;
    public CameraRotation cameraScript;
    public Transform bodyTarget;
    public Transform SpineIK;

    private CharacterController controller;
    private Vector3 velocity;

    private bool canMove;
    private bool isWalking;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isSprinting;

    void Start()
    {
        isWalking = false;
        isSprinting = false;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        StateUpdate();
        ApplyGravity();
        Move();
        if (cameraScript.isAiming)
            BodyRotation();
        else
            ResetBodyRotation();

    }

    void Move()
    {
        speed = currentSpeed / maxSpeed;
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        animator.SetFloat("Speed", 0);
        animator.SetBool("onGround", isGrounded);

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        Vector3 move = Vector3.zero;

        if (input.magnitude >= 0.1f)
        {
            animator.SetFloat("Speed", speed);
            Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = cameraTransform.right;
            Vector3 moveDir = camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal");
            moveDir.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            move = moveDir * currentSpeed;
        }

        Vector3 totalMove = move * Time.deltaTime;
        totalMove.y = velocity.y * Time.deltaTime;
        controller.Move(totalMove);
    }

    void ApplyGravity()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && playerParam._stamina > 10)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
    }

    void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            isWalking = !isWalking; // переключаем состояние

        if (Input.GetKey(KeyCode.LeftShift) && !isWalking && playerParam._stamina > 0)
            isSprinting = true;
        else
            isSprinting = false;

        // --- Выбор скорости ---
        if (isSprinting)
            currentSpeed = maxSpeed;
        else if (isWalking)
            currentSpeed = walkSpeed;
        else
            currentSpeed = standardSpeed;
    }

    void BodyRotation()
    {
            Ray desiredTargetRay = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            Vector3 desiredTargetPosition = desiredTargetRay.origin + desiredTargetRay.direction * 10;
            bodyTarget.position = desiredTargetPosition;
        
    }

    void ResetBodyRotation()
    {
        bodyTarget.position = spineBone.position;
    }
}